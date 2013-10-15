using System;
using Moq;
using NUnit.Framework;
using Radvill.Advisor.Internal.Services;
using Radvill.Advisor.Public;
using Radvill.Models.AdviseModels;
using Radvill.Services.DataFactory;
using Radvill.Services.DataFactory.Repositories;
using Radvill.Services.Sockets;

namespace Radvill.Tests.Advisor.AdviseManagerTests
{
    [TestFixture]
    public class SubmitAnswerTests
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