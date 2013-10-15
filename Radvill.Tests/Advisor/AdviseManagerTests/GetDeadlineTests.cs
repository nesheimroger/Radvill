using System;
using Moq;
using NUnit.Framework;
using Radvill.Advisor.Internal.Services;
using Radvill.Advisor.Public;
using Radvill.Models.AdviseModels;
using Radvill.Services.DataFactory;
using Radvill.Services.Sockets;

namespace Radvill.Tests.Advisor.AdviseManagerTests
{
    [TestFixture]
    public class GetDeadlineTests
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

    }
}