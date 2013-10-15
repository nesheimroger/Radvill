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
    public class StartAnswerTests
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
    }
}