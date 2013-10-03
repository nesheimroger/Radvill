using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Radvill.Advisor.Private.Services;
using Radvill.Advisor.Public;
using Radvill.DataFactory.Public.Repositories;
using Radvill.Models.AdviseModels;
using Radvill.Models.UserModels;
using Radvill.Services.DataFactory;
using Radvill.Services.DataFactory.Repositories;
using Radvill.Services.Events;

namespace Radvill.Tests.Advisor
{
    [TestFixture]
    public class AdviseManagerTests
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
        public void SubmitQuestion_ShouldAddNewPendingQuestion_IfAdvisorIsAvailable()
        {
            //Arrange
            _advisorLocator.Setup(x => x.GetNextInLine()).Returns(new User());
            _dataFactory.Setup(x => x.UserRepository.GetByID(It.IsAny<int>())).Returns(new User());
            _dataFactory.Setup(x => x.CategoryRepository.GetByID(It.IsAny<int>())).Returns(new Category());
            var pendingQuestionRepository = new Mock<IPendingQuestionRepository>();
            _dataFactory.Setup(x => x.PendingQuestionRepository).Returns(pendingQuestionRepository.Object);

            //Act
            var result = _adviseManager.SubmitQuestion(1, 1, "What is this?");

            //Assert
            Assert.That(result, Is.True);
            pendingQuestionRepository.Verify(x => x.Insert(It.IsAny<PendingQuestion>()), Times.Once);
            _dataFactory.Verify(x => x.Commit(), Times.Once);
            _eventManager.Verify(x => x.QuestionAssigned(It.IsAny<PendingQuestion>()), Times.Once);
        }

        [Test]
        public void SubmitQuestion_ShouldReturnFalse_IfNotAdvisorIsAvailable()
        {
            //Arrange
            _advisorLocator.Setup(x => x.GetNextInLine()).Returns((User)null);
            _dataFactory.Setup(x => x.UserRepository.GetByID(It.IsAny<int>())).Returns(new User());
            _dataFactory.Setup(x => x.CategoryRepository.GetByID(It.IsAny<int>())).Returns(new Category());
            var pendingQuestionRepository = new Mock<IPendingQuestionRepository>();
            _dataFactory.Setup(x => x.PendingQuestionRepository).Returns(pendingQuestionRepository.Object);

            //Act
            var result = _adviseManager.SubmitQuestion(1, 1, "What is this?");

            //Assert
            Assert.That(result, Is.False);
            pendingQuestionRepository.Verify(x => x.Insert(It.IsAny<PendingQuestion>()), Times.Never);
            _dataFactory.Verify(x => x.Commit(), Times.Never);
            _eventManager.Verify(x => x.QuestionAssigned(It.IsAny<PendingQuestion>()), Times.Never);
        }

        [Test]
        public void PassQuestion_ShouldSetPassStatus_ToPendingQuestion()
        {
            //Arrange

            var pending = new PendingQuestion
                {
                    Status = null
                };

            _dataFactory.Setup(x => x.PendingQuestionRepository.GetByUserIDAndQuestionId(It.IsAny<int>(), It.IsAny<int>())).Returns(pending);

            //Act
            _adviseManager.PassQuestion(1, 1);

            //Assert
            Assert.That(pending.Status, Is.False);
            _dataFactory.Verify(x => x.PendingQuestionRepository.Update(It.IsAny<PendingQuestion>()), Times.Once);
            _dataFactory.Verify(x => x.Commit(), Times.Once);
            _eventManager.Verify(x => x.QuestionPassed(It.IsAny<PendingQuestion>()), Times.Once);

        }

        [Test]
        public void PassQuestion_ShouldAddNewPendingQuestion_IfAdvisorIsAvailable()
        {
            //Arrange
            var pendingQuestionRepository = new Mock<IPendingQuestionRepository>();
            pendingQuestionRepository.Setup(x => x.GetByUserIDAndQuestionId(It.IsAny<int>(), It.IsAny<int>())).Returns(new PendingQuestion());
            _dataFactory.Setup(x => x.PendingQuestionRepository).Returns(pendingQuestionRepository.Object);
            _dataFactory.Setup(x => x.QuestionRepository.GetByID(It.IsAny<int>())).Returns(new Question());
            _advisorLocator.Setup(x => x.GetNextInLine(It.IsAny<int>())).Returns(new User());
            
            //Act
            var result = _adviseManager.PassQuestion(1,1);

            //Assert
            Assert.That(result, Is.True);
            pendingQuestionRepository.Verify(x => x.Insert(It.IsAny<PendingQuestion>()), Times.Once);
            _dataFactory.Verify(x => x.Commit(), Times.Once);
            _eventManager.Verify(x => x.QuestionAssigned(It.IsAny<PendingQuestion>()), Times.Once);
            _eventManager.Verify(x => x.QuestionPassed(It.IsAny<PendingQuestion>()), Times.Once);

        }

        [Test]
        public void PassQuestion_ShouldReturnFalse_IfAdvisorIsNotAvailable()
        {
            //Arrange
            var pendingQuestionRepository = new Mock<IPendingQuestionRepository>();
            pendingQuestionRepository.Setup(x => x.GetByUserIDAndQuestionId(It.IsAny<int>(), It.IsAny<int>())).Returns(new PendingQuestion());
            _dataFactory.Setup(x => x.PendingQuestionRepository).Returns(pendingQuestionRepository.Object);
            _dataFactory.Setup(x => x.QuestionRepository.GetByID(It.IsAny<int>())).Returns(new Question());
            _advisorLocator.Setup(x => x.GetNextInLine(It.IsAny<int>())).Returns((User)null);

            //Act
            var result = _adviseManager.PassQuestion(1, 1);

            //Assert
            Assert.That(result, Is.False);
            pendingQuestionRepository.Verify(x => x.Update(It.IsAny<PendingQuestion>()), Times.Once);
            _dataFactory.Verify(x => x.Commit(), Times.Once);
            _eventManager.Verify(x => x.QuestionAssigned(It.IsAny<PendingQuestion>()), Times.Never);
            _eventManager.Verify(x => x.QuestionPassed(It.IsAny<PendingQuestion>()), Times.Once);
        }
    }
}
