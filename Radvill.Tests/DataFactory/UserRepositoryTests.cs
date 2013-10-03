using System.Linq;
using Moq;
using NUnit.Framework;
using Radvill.DataFactory.Internal.Services;
using Radvill.DataFactory.Public.Repositories;
using Radvill.Models.AdviseModels;
using Radvill.Models.UserModels;
using Radvill.Services.DataFactory;
using Radvill.Tests.TestHelpers;

namespace Radvill.Tests.DataFactory
{
    [TestFixture]
    public class UserRepositoryTests
    {
        [Test]
         public void GetAvailableUsers_ShouldReturnUsers_WhoIsntAnswereringAQuestion()
         {
             //Arrange
             var dbMock = new Mock<IRadvillContext>();

             var userOne = new User
                 {
                     ID = 1,
                     Connected = true
                 };

             var userTwo = new User
                 {
                     ID = 2,
                     Connected = true
                 };

             var userDbSet = new FakeDbSet<User>
                 {
                     userOne,
                     userTwo
                 };

             dbMock.Setup(x => x.Users).Returns(userDbSet);

             var pendingQuestion = new PendingQuestion
                 {
                     ID = 1,
                     User = userOne
                 };

             var pendingQuestionDbSet = new FakeDbSet<PendingQuestion>
                 {
                     pendingQuestion
                 };

             dbMock.Setup(x => x.PendingQuestions).Returns(pendingQuestionDbSet);
            dbMock.Setup(x => x.Set<User>()).Returns(userDbSet);

             var userRepository = new UserRepository(dbMock.Object);

             //Act
             var result = userRepository.GetAvailableUsers();

             //Assert
             Assert.That(result, Is.Not.Null);
             Assert.That(result.Count(), Is.EqualTo(1));
             Assert.That(result.First().ID, Is.EqualTo(userTwo.ID));

         }

        [Test]
        public void GetAvailableUsers_ShouldOnlyReturnConnectedUsers()
        {

            //Arrange
            var dbMock = new Mock<IRadvillContext>();

            var userOne = new User
            {
                ID = 1,
                Connected = false
            };

            var userTwo = new User
            {
                ID = 2,
                Connected = true
            };

            var userThree = new User
            {
                ID = 3,
                Connected = true
            };

            var userFour = new User
            {
                ID = 4,
                Connected = true
            };
            var userDbSet = new FakeDbSet<User>
                 {
                     userOne,
                     userTwo,
                     userThree,
                     userFour
                 };

            dbMock.Setup(x => x.Users).Returns(userDbSet);
            dbMock.Setup(x => x.PendingQuestions).Returns(new FakeDbSet<PendingQuestion>());
            dbMock.Setup(x => x.Set<User>()).Returns(userDbSet);

            var userRepository = new UserRepository(dbMock.Object);

            //Act
            var result = userRepository.GetAvailableUsers();

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result.Any(x => !x.Connected), Is.False);
        }
    }
}