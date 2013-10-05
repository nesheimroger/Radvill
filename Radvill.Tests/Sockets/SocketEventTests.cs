using NUnit.Framework;
using Radvill.Models.AdviseModels;
using Radvill.Sockets.Internal;

namespace Radvill.Tests.Sockets
{
    [TestFixture]
    public class SocketEventTests
    {
         [Test]
         public void QuestionAssigned_ShouldReturnCorrectJson()
         {
             //Arrange
             const string expectedJson = "[\"QuestionAssigned\",{\"ID\":1}]";
             var pendingQuestion = new PendingQuestion {ID = 1};

             //Act
             var result = SocketEvent.QuestionAssigned(pendingQuestion);

             //Assert
             Assert.That(result, Is.EqualTo(expectedJson));
         }
    }
}