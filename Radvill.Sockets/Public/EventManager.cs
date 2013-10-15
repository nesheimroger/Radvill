using Newtonsoft.Json;
using Radvill.Models.AdviseModels;
using Radvill.Services.Sockets;
using Radvill.Sockets.Internal;

namespace Radvill.Sockets.Public
{
    public class EventManager : IEventManager
    {
        private readonly ISocketManager _socketManager;

        public EventManager(ISocketManager socketManager)
        {
            _socketManager = socketManager;
        }

        public void QuestionAssigned(PendingQuestion pendingQuestion)
        {
            var client = _socketManager.GetClient(pendingQuestion.User.Email);
            if (client != null)
            {
                client.Send(SocketEvent.QuestionAssigned(pendingQuestion));
            }
        }

        public void AnswerStarted(PendingQuestion pendingQuestion)
        {
            var client = _socketManager.GetClient(pendingQuestion.Question.User.Email);
            if (client != null)
            {
                client.Send(SocketEvent.AnswerStarted(pendingQuestion));
            }
        }

        public void AnswerSubmitted(Answer answer)
        {
            var client = _socketManager.GetClient(answer.Question.User.Email);
            if (client != null)
            {
                client.Send(SocketEvent.AnswerSubmitted(answer));
            }
        }

        public void AllRecipientsPassed(Question question)
        {
            var client = _socketManager.GetClient(question.User.Email);
            if (client != null)
            {
                client.Send(SocketEvent.AllRecipientsPassed(question));
            }
        }

        public void AnswerEvaluated(Answer answer)
        {
            var client = _socketManager.GetClient(answer.User.Email);
            if (client != null)
            {
                client.Send(SocketEvent.AnswerEvaluated(answer));
            }
        }
    }
}