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
using Radvill.DataFactory.Public.Repositories;
using Radvill.Models.AdviseModels;
using Radvill.Models.UserModels;
using Radvill.Services.DataFactory;
using Radvill.Services.DataFactory.Repositories;
using Radvill.Services.Sockets;

namespace Radvill.Tests.Advisor
{
    [TestFixture]
    public class AdviseManagerTests
    {
        private Mock<IAdvisorLocator> _advisorLocator;
        private Mock<IDataFactory> _dataFactory;
        private Mock<IEventManager> _eventManager;
        private Mock<ISocketManager> _socketManager;
        private AdviseManager _adviseManager;

        [SetUp]
        public void Setup()
        {
            _advisorLocator = new Mock<IAdvisorLocator>();
            _dataFactory = new Mock<IDataFactory>();
            _eventManager = new Mock<IEventManager>();
            _socketManager = new Mock<ISocketManager>();
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

            //Act
            _adviseManager.PassQuestion(pending);

            //Assert
            Assert.That(pending.Status, Is.False);
            _dataFactory.Verify(x => x.PendingQuestionRepository.Update(pending), Times.Once);
            _dataFactory.Verify(x => x.Commit(), Times.Once);

        }

        [Test]
        public void PassQuestion_ShouldAddNewPendingQuestion_IfAdvisorIsAvailable()
        {

            var pending = new PendingQuestion {Question = new Question()};
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
            _dataFactory.Verify(x => x.Commit(), Times.Once);
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
            _dataFactory.Verify(x => x.Commit(), Times.Once);
            _eventManager.Verify(x => x.QuestionAssigned(It.IsAny<PendingQuestion>()), Times.Never);
            _eventManager.Verify(x => x.AllRecipientsPassed(It.IsAny<Question>()), Times.Once);
        }

        

        [Test]
        public void StartAnswer_ShouldUpdateStatus_ToPendingQuestion_WhenSuccessfull()
        {
            //Arrange
            var pending = new PendingQuestion
            {
                TimeStamp = DateTime.Now.AddSeconds(-Configuration.Timeout.Respond + 5) //5 seconds to spare
            };
            var pendingQuestionRepository = new Mock<IPendingQuestionRepository>();
            pendingQuestionRepository.Setup(x => x.GetByID(It.IsAny<int>())).Returns(pending);
            _dataFactory.Setup(x => x.PendingQuestionRepository).Returns(pendingQuestionRepository.Object);

            //Act
            _adviseManager.StartAnswer(pending);

            //Assert
            Assert.That(pending.Status, Is.True);
            pendingQuestionRepository.Verify(x => x.Update(pending), Times.Once);
            _dataFactory.Verify(x => x.Commit(), Times.Once);

        }


        [Test]
        public void StartAnswer_ShouldNotifyEventManager_WhenSuccessfull()
        {
            //Arrange
            var pending = new PendingQuestion
            {
                TimeStamp = DateTime.Now.AddSeconds(-Configuration.Timeout.Respond + 5) //5 seconds to spare
            };
            var pendingQuestionRepository = new Mock<IPendingQuestionRepository>();
            pendingQuestionRepository.Setup(x => x.GetByID(It.IsAny<int>())).Returns(pending);
            _dataFactory.Setup(x => x.PendingQuestionRepository).Returns(pendingQuestionRepository.Object);

            //Act
            _adviseManager.StartAnswer(pending);

            //Assert
            _eventManager.Verify(x => x.AnswerStarted(pending), Times.Once);
        }

        [Test]
        public void StartAnswer_ShouldReturnTrue_WhenSuccessfull()
        {
            //Arrange
            var pending = new PendingQuestion
            {
                TimeStamp = DateTime.Now.AddSeconds(-Configuration.Timeout.Respond + 5) //5 seconds to spare
            };
            var pendingQuestionRepository = new Mock<IPendingQuestionRepository>();
            pendingQuestionRepository.Setup(x => x.GetByID(It.IsAny<int>())).Returns(pending);
            _dataFactory.Setup(x => x.PendingQuestionRepository).Returns(pendingQuestionRepository.Object);

            //Act
            var result = _adviseManager.StartAnswer(pending);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void StartAnswer_ShouldNotNotifyEventManagerOrUpdateStatus_AndReturnFalse_IfUserWasTooLate()
        {
            

            var pending = new PendingQuestion
                {
                    TimeStamp = DateTime.Now.AddSeconds(-Configuration.Timeout.Respond - 5) //5 seconds too late
                };
            var pendingQuestionRepository = new Mock<IPendingQuestionRepository>();
            pendingQuestionRepository.Setup(x => x.GetByID(It.IsAny<int>())).Returns(pending);
            _dataFactory.Setup(x => x.PendingQuestionRepository).Returns(pendingQuestionRepository.Object);

            //Act
            var result = _adviseManager.StartAnswer(pending);

            //Assert
            Assert.That(result, Is.False);
            pendingQuestionRepository.Verify(x => x.Update(pending), Times.Never);
            _dataFactory.Verify(x => x.Commit(), Times.Never);
            _eventManager.Verify(x => x.AnswerStarted(pending), Times.Never);
        }

        [Test]
        public void GetDeadLine_ShouldReturnCorrectDateTime_ForRespond()
        {
            //Arrange
            var now = DateTime.Now;
            var expected = now.AddSeconds(Configuration.Timeout.Respond);

            var pending = new PendingQuestion
                {
                    Status = null,
                    TimeStamp = now
                };

            //Act
            var result = _adviseManager.GetDeadline(pending);

            //Arrange
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetDeadline_ShouldReturnCorrectDateTime_ForAnswer()
        {
            //Arrange
            var now = DateTime.Now;
            var expected = now.AddSeconds(Configuration.Timeout.Respond + Configuration.Timeout.Answer);

            var pending = new PendingQuestion
            {
                Status = true,
                TimeStamp = now
            };

            //Act
            var result = _adviseManager.GetDeadline(pending);

            //Arrange
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void PassQuestionForUser_ShouldPassQuestionsThatAreUnanswered()
        {
            var shouldPass = new PendingQuestion {Status = null, Question = new Question(){ID = 1}};
            var shouldPass2 = new PendingQuestion {Status = true, Question = new Question(){ID = 1}};
            var shouldNotPass = new PendingQuestion {Status = true, Answer = new Answer(), Question = new Question(){ID = 1}};
            var shouldNotPass2 = new PendingQuestion {Status = false, Question = new Question(){ID = 1}};


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
        
        [Test]
        public void SubmitAnswer_ShouldReturnTrue_WhenSuccessfull()
        {
            //Arrange
            var pending = new PendingQuestion
            {
                TimeStamp = DateTime.Now.AddSeconds(-Configuration.Timeout.Respond + 5), //5 seconds to spare
            };
            const string answer = "Answer";
            var answerRepo = new Mock<IAnswerRepository>();
            var pendingQuestionRepo = new Mock<IPendingQuestionRepository>();
            _dataFactory.Setup(x => x.AnswerRepository).Returns(answerRepo.Object);
            _dataFactory.Setup(x => x.PendingQuestionRepository).Returns(pendingQuestionRepo.Object);

            //Act
            var result = _adviseManager.SubmitAnswer(pending, answer);

            //Assert
            Assert.That(result, Is.True);
            Assert.That(pending.Answer, Is.Not.Null);
            answerRepo.Verify(x => x.Insert(It.IsAny<Answer>()), Times.Once);
            pendingQuestionRepo.Verify(x => x.Update(pending), Times.Once);
            _dataFactory.Verify(x => x.Commit(), Times.Once);
            _eventManager.Verify(x => x.AnswerSubmitted(It.IsAny<Answer>()), Times.Once);
        }

        [Test]
        public void SubmitAnswer_ShouldReturnFalse_WhenDeadlineHasPassed()
        {
            //Arrange
            var pending = new PendingQuestion
            {
                TimeStamp = DateTime.Now.AddSeconds(-Configuration.Timeout.Respond - 5) //5 seconds over due
            };
            const string answer = "Answer";
            var answerRepo = new Mock<IAnswerRepository>();
            _dataFactory.Setup(x => x.AnswerRepository).Returns(answerRepo.Object);

            //Act
            var result = _adviseManager.SubmitAnswer(pending, answer);

            //Assert
            Assert.That(result, Is.False);
            answerRepo.Verify(x => x.Insert(It.IsAny<Answer>()), Times.Never);
            _dataFactory.Verify(x => x.Commit(), Times.Never);
            _eventManager.Verify(x => x.AnswerSubmitted(It.IsAny<Answer>()), Times.Never);

        }

        [Test]
        public void SubmitAnswer_ShouldNotAddMoreThenOneAnswer_EachPendingQuestion()
        {
            //Arrange
            var pending = new PendingQuestion
            {
                TimeStamp = DateTime.Now.AddSeconds(-Configuration.Timeout.Respond + 5), //5 seconds to spare
                Answer = new Answer()
            };
            const string answer = "Answer";
            var answerRepo = new Mock<IAnswerRepository>();
            _dataFactory.Setup(x => x.AnswerRepository).Returns(answerRepo.Object);

            //Act
            var result = _adviseManager.SubmitAnswer(pending, answer);

            //Assert
            Assert.That(result, Is.False);
            answerRepo.Verify(x => x.Insert(It.IsAny<Answer>()), Times.Never);
            _dataFactory.Verify(x => x.Commit(), Times.Never);
            _eventManager.Verify(x => x.AnswerSubmitted(It.IsAny<Answer>()), Times.Never);
        }
    }
}
