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
    public class SubmitQuestionTests
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
            _advisorLocator.Setup(x => x.GetNextInLine(It.IsAny<int>())).Returns(new User());
            _dataFactory.Setup(x => x.UserRepository.GetByID(It.IsAny<int>())).Returns(new User());
            _dataFactory.Setup(x => x.CategoryRepository.GetByID(It.IsAny<int>())).Returns(new Category());

            var questionRepository = new Mock<IQuestionRepository>();
            _dataFactory.Setup(x => x.QuestionRepository).Returns(questionRepository.Object);
            var pendingQuestionRepository = new Mock<IPendingQuestionRepository>();
            _dataFactory.Setup(x => x.PendingQuestionRepository).Returns(pendingQuestionRepository.Object);

            //Act
            var result = _adviseManager.SubmitQuestion(1, 1, "What is this?");

            //Assert
            Assert.That(result, Is.True);
            pendingQuestionRepository.Verify(x => x.Insert(It.IsAny<PendingQuestion>()), Times.Once);
            _dataFactory.Verify(x => x.Commit(), Times.Exactly(2));
            _eventManager.Verify(x => x.QuestionAssigned(It.IsAny<PendingQuestion>()), Times.Once);
        }

        [Test]
        public void SubmitQuestion_ShouldReturnFalse_IfNotAdvisorIsAvailable()
        {
            //Arrange
            _advisorLocator.Setup(x => x.GetNextInLine(It.IsAny<int>())).Returns((User)null);
            _dataFactory.Setup(x => x.UserRepository.GetByID(It.IsAny<int>())).Returns(new User());
            _dataFactory.Setup(x => x.CategoryRepository.GetByID(It.IsAny<int>())).Returns(new Category());
            var questionRepository = new Mock<IQuestionRepository>();
            _dataFactory.Setup(x => x.QuestionRepository).Returns(questionRepository.Object);
            var pendingQuestionRepository = new Mock<IPendingQuestionRepository>();
            _dataFactory.Setup(x => x.PendingQuestionRepository).Returns(pendingQuestionRepository.Object);

            //Act
            var result = _adviseManager.SubmitQuestion(1, 1, "What is this?");

            //Assert
            Assert.That(result, Is.False);
            pendingQuestionRepository.Verify(x => x.Insert(It.IsAny<PendingQuestion>()), Times.Never);
            questionRepository.Verify(x => x.Insert(It.IsAny<Question>()), Times.Once);
            questionRepository.Verify(x => x.Delete(It.IsAny<Question>()), Times.Once);
            _dataFactory.Verify(x => x.Commit(), Times.Exactly(2));
            _eventManager.Verify(x => x.QuestionAssigned(It.IsAny<PendingQuestion>()), Times.Never);
        }
    }
}