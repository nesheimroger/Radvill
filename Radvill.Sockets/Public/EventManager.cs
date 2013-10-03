using Radvill.Models.AdviseModels;
using Radvill.Services.Sockets;

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
                client.Send("QuestionAssigned");
            }
        }

        public void AnswerStarted(PendingQuestion pendingQuestion)
        {
            var client = _socketManager.GetClient(pendingQuestion.Question.User.Email);
            if (client != null)
            {
                client.Send("AnswerStarted");
            }
        }

        public void AnswerSubmitted(Answer answer)
        {
            var client = _socketManager.GetClient(answer.Question.User.Email);
            if (client != null)
            {
                client.Send("AnswerSubmitted");
            }
        }
    }
}