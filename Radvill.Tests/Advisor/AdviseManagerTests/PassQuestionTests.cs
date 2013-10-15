using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Radvill.Advisor.Internal.Services;
using Radvill.Advisor.Public;
using Radvill.Models.AdviseModels;
using Radvill.Models.UserModels;
using Radvill.Services.DataFactory;
using Radvill.Services.DataFactory.Repositories;
using Radvill.Services.Sockets;

namespace Radvill.Tests.Advisor.AdviseManagerTests
{
    [TestFixture]
    public class PassQuestionTests
    {
        private Mock<IAdvisorLocator> _advisorLocator;
        private Mock<IDataFactory> _dataFactory;
        private Mock<IEventManager> _eventManager;
        private AdviseManager _adviseManager;

        [SetUp]
        public void Setup()
        {
            _advisorLocator = new Mock<IAdvisorLocator>();
            _dataFactory = new Mock<IDataFactory>();
            _eventManager = new Mock<IEventManager>();
            _adviseManager = new AdviseManager(_dataFactory.Object, _advisorLocator.Object, _eventManager.Object);
        }

        [TearDown]
        public void Teardown()
        {

        }

        [Test]
        public void PassQuestion_ShouldSetPassStatus_ToPendingQuestion()
        {
            //Arrange

            var pending = new PendingQuestion
            {
                Question = new Question(),
                Status = null
            };

            _dataFactory.Setup(x => x.PendingQuestionRepository.GetByID(It.IsAny<int>())).Returns(pending);
            _dataFactory.Setup(x => x.QuestionRepository).Returns(new Mock<IQuestionRepository>().Object);

            //Act
            _adviseManager.PassQuestion(pending);

            //Assert
            Assert.That(pending.Status, Is.False);
            _dataFactory.Verify(x => x.PendingQuestionRepository.Update(pending), Times.Once);
            _dataFactory.Verify(x => x.Commit(), Times.Exactly(2));

        }

        [Test]
        public void PassQuestion_ShouldAddNewPendingQuestion_IfAdvisorIsAvailable()
        {

            var pending = new PendingQuestion { Question = new Question() };
            //Arrange
            var pendingQuestionRepository = new Mock<IPendingQuestionRepository>();
            pendingQuestionRepository.Setup(x => x.GetByID(It.IsAny<int>())).Returns(pending);
            _dataFactory.Setup(x => x.PendingQuestionRepository).Returns(pendingQuestionRepository.Object);
            _dataFactory.Setup(x => x.QuestionRepository.GetByID(It.IsAny<int>())).Returns(new Question());
            _advisorLocator.Setup(x => x.GetNextInLine(It.IsAny<int>())).Returns(new User());

            //Act
            _adviseManager.PassQuestion(pending);

            //Assert
            pendingQuestionRepository.Verify(x => x.Update(pending), Times.Once);
            pendingQuestionRepository.Verify(x => x.Insert(It.IsAny<PendingQuestion>()), Times.Once);
            _dataFactory.Verify(x => x.Commit(), Times.Exactly(2));
            _eventManager.Verify(x => x.QuestionAssigned(It.IsAny<PendingQuestion>()), Times.Once);

        }

        [Test]
        public void PassQuestion_ShouldNotifyEventManager_IfAdvisorIsNotAvailable()
        {
            //Arrange
            var pending = new PendingQuestion { Question = new Question() };
            var pendingQuestionRepository = new Mock<IPendingQuestionRepository>();
            pendingQuestionRepository.Setup(x => x.GetByID(It.IsAny<int>())).Returns(pending);
            _dataFactory.Setup(x => x.PendingQuestionRepository).Returns(pendingQuestionRepository.Object);
            _dataFactory.Setup(x => x.QuestionRepository.GetByID(It.IsAny<int>())).Returns(new Question());
            _advisorLocator.Setup(x => x.GetNextInLine(It.IsAny<int>())).Returns((User)null);

            //Act
            _adviseManager.PassQuestion(pending);

            //Assert
            pendingQuestionRepository.Verify(x => x.Update(pending), Times.Once);
            _dataFactory.Verify(x => x.Commit(), Times.Exactly(2));
            _eventManager.Verify(x => x.QuestionAssigned(It.IsAny<PendingQuestion>()), Times.Never);
            _eventManager.Verify(x => x.AllRecipientsPassed(It.IsAny<Question>()), Times.Once);
        }

        [Test]
        public void PassQuestionForUser_ShouldPassQuestionsThatAreUnanswered()
        {
            var shouldPass = new PendingQuestion { Status = null, Question = new Question() { ID = 1 } };
            var shouldPass2 = new PendingQuestion { Status = true, Question = new Question() { ID = 1 } };
            var shouldNotPass = new PendingQuestion { Status = true, Answer = new Answer(), Question = new Question() { ID = 1 } };
            var shouldNotPass2 = new PendingQuestion { Status = false, Question = new Question() { ID = 1 } };


            var user = new User
            {
                PendingQuestions = new Collection<PendingQuestion>
                        {
                            shouldPass,
                            shouldPass2,
                            shouldNotPass,
                            shouldNotPass2
                        }
            };

            _dataFactory.Setup(x => x.UserRepository.GetUserByEmail(It.IsAny<string>())).Returns(user);
            _dataFactory.Setup(x => x.PendingQuestionRepository).Returns(new Mock<IPendingQuestionRepository>().Object);
            _advisorLocator.Setup(x => x.GetNextInLine(It.IsAny<int>())).Returns(new User());
            _adviseManager.PassQuestionForUser("email");

            Assert.That(shouldPass.Status, Is.False);
            Assert.That(shouldPass2.Status, Is.False);
            Assert.That(shouldNotPass.Status, Is.True);
            Assert.That(shouldNotPass2.Status, Is.False);
        }
    }
}
