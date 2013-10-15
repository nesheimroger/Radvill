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
    public class EvaluateAnswerTests
    {
        private Mock<IAdvisorLocator> _advisorLocator;
        private Mock<IDataFactory> _dataFactory;
        private Mock<IEventManager> _eventManager;
        private Mock<IAnswerRepository> _answerRepository;
        private Mock<IQuestionRepository> _questionRepository;
        private Mock<IPendingQuestionRepository> _pendingQuestionRepository;
        private AdviseManager _adviseManager;

        [SetUp]
        public void Setup()
        {
            _advisorLocator = new Mock<IAdvisorLocator>();
            _dataFactory = new Mock<IDataFactory>();
            _eventManager = new Mock<IEventManager>();
            _answerRepository = new Mock<IAnswerRepository>();
            _questionRepository = new Mock<IQuestionRepository>();
            _pendingQuestionRepository = new Mock<IPendingQuestionRepository>();
            _dataFactory.Setup(x => x.AnswerRepository).Returns(_answerRepository.Object);
            _dataFactory.Setup(x => x.QuestionRepository).Returns(_questionRepository.Object);
            _dataFactory.Setup(x => x.PendingQuestionRepository).Returns(_pendingQuestionRepository.Object);
            _adviseManager = new AdviseManager(_dataFactory.Object, _advisorLocator.Object, _eventManager.Object);
            
        }

        [TearDown]
        public void Teardown()
        {

        }

        [Test]
        public void AcceptAnswer_ShouldSetAcceptedStatus()
        {
            //Arrange
            var answer = new Answer {Accepted = null};

            //Act
            _adviseManager.AcceptAnswer(answer);

            //Assert
            Assert.That(answer.Accepted , Is.True);
            _answerRepository.Verify(x => x.Update(answer), Times.Once);
            _dataFactory.Verify(x => x.Commit(), Times.Once);

        }

        [Test]
        public void AcceptAnswer_ShouldNotifyAdvisor()
        {
            //Arrange
            var answer = new Answer { Accepted = null };

            //Act
            _adviseManager.AcceptAnswer(answer);

            //Assert
            _eventManager.Verify(x => x.AnswerEvaluated(answer), Times.Once);
        }

        [Test]
        public void AcceptAnswer_ShouldSetDeclinedStatus()
        {
            //Arrange
            var answer = new Answer { Accepted = null, Question = new Question { ID = 1 } };
            _advisorLocator.Setup(x => x.GetNextInLine(It.IsAny<int>())).Returns(new User());

            //Act
            _adviseManager.DeclineAnswer(answer);

            //Assert
            Assert.That(answer.Accepted, Is.False);
            _answerRepository.Verify(x => x.Update(answer), Times.Once);
            _dataFactory.Verify(x => x.Commit(), Times.Exactly(2));

        }

        [Test]
        public void DeclineAnswer_ShouldNotifyAdvisor()
        {
            //Arrange
            var answer = new Answer { Accepted = null, Question = new Question { ID = 1 } };

            //Act
            _adviseManager.DeclineAnswer(answer);

            //Assert
            _eventManager.Verify(x => x.AnswerEvaluated(answer), Times.Once);
        }

        [Test]
        public void DeclineAnswer_ShouldPassTheQuestionOnToANewAdvisor()
        {
            //Arrange
            var answer = new Answer { Accepted = null, Question = new Question { ID = 1 } };
            _advisorLocator.Setup(x => x.GetNextInLine(It.IsAny<int>())).Returns(new User());
            //Act
            var result = _adviseManager.DeclineAnswer(answer);

            //Assert
            Assert.That(result, Is.True);
            _pendingQuestionRepository.Verify(x => x.Insert(It.IsAny<PendingQuestion>()), Times.Once);
            _dataFactory.Verify(x => x.Commit(), Times.Exactly(2));
        }

        [Test]
        public void DeclineAnswer_ShouldReturnFalse_IfNoAdvisorIsAvailable()
        {
            //Arrange
            var answer = new Answer { Accepted = null, Question = new Question { ID = 1 } };
            _advisorLocator.Setup(x => x.GetNextInLine(It.IsAny<int>())).Returns((User)null);
            //Act
            var result = _adviseManager.DeclineAnswer(answer);

            //Assert
            Assert.That(result, Is.False);
        }
    }
}