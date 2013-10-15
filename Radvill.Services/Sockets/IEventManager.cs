using Radvill.Models.AdviseModels;

namespace Radvill.Services.Sockets
{
    public interface IEventManager
    {
        void QuestionAssigned(PendingQuestion pendingQuestion);
        void AnswerStarted(PendingQuestion pendingQuestion);
        void AnswerSubmitted(Answer answer);
        void AllRecipientsPassed(Question question);
        void AnswerEvaluated(Answer answer);
    }
}