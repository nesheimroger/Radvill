using Radvill.Models.AdviseModels;

namespace Radvill.Services.Events
{
    public interface IEventManager
    {
        void QuestionAssigned(PendingQuestion pendingQuestion);
        void QuestionPassed(PendingQuestion pendingQuestion);
        void AnswerStarted(PendingQuestion pendingQuestion);
        void AnswerSubmitted(Answer answer);

    }
}