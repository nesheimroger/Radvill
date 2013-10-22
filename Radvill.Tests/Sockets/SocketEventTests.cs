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

         [Test]
         public void AnswerSubmitted_ShouldReturnCorrectJson()
         {
             //Arrange
             const string expectedJson = "[\"AnswerSubmitted\",{\"AnswerID\":1,\"QuestionID\":1}]";
             var answer = new Answer { ID = 1, Question = new Question{ID = 1}};

             //Act
             var result = SocketEvent.AnswerSubmitted(answer);

             //Assert
             Assert.That(result, Is.EqualTo(expectedJson));
         }

         [Test]
         public void AnswerStarted_ShouldReturnCorrectJson()
         {
             //Arrange
             const string expectedJson = "[\"AnswerStarted\",{\"ID\":1}]";
             var pendingQuestion = new PendingQuestion { ID = 1, Question = new Question{ID = 1}};

             //Act
             var result = SocketEvent.AnswerStarted(pendingQuestion);

             //Assert
             Assert.That(result, Is.EqualTo(expectedJson));
         }

         [Test]
         public void AllRecipientsPassed_ShouldReturnCorrectJson()
         {
             //Arrange
             const string expectedJson = "[\"AllRecipientsPassed\",{\"ID\":1}]";
             var question = new Question {ID = 1};

             //Act
             var result = SocketEvent.AllRecipientsPassed(question);

             //Assert
             Assert.That(result, Is.EqualTo(expectedJson));
         }

         [Test]
         public void AnswerEvaluated_ShouldReturnCorrectJson()
         {
             //Arrange
             const string expectedJson = "[\"AnswerEvaluated\",{\"ID\":1,\"Accepted\":true}]";
             var answer = new Answer { ID = 1, Accepted = true};

             //Act
             var result = SocketEvent.AnswerEvaluated(answer);

             //Assert
             Assert.That(result, Is.EqualTo(expectedJson));
         }
    }
}