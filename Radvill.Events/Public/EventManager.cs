using Radvill.Models.AdviseModels;
using Radvill.Services.Events;

namespace Radvill.Events.Public
{
    public class EventManager : IEventManager, IEventSubscriber
    {


        public void QuestionAssigned(PendingQuestion pendingQuestion)
        {
            Question_Submitted(pendingQuestion);
        }

        public void QuestionPassed(PendingQuestion pendingQuestion)
        {
            Question_Passed(pendingQuestion);
        }

        public void AnswerStarted(PendingQuestion pendingQuestion)
        {
            Answer_Started(pendingQuestion);
        }

        public void AnswerSubmitted(Answer answer)
        {
            Answer_Submitted(answer);
        }


        public event QuestionAssignedEventHandler Question_Submitted;
        public event QuestionPassedEventHandler Question_Passed;
        public event AnswerStartedEventHandler Answer_Started;
        public event AnswerSubmittedEventHandler Answer_Submitted;
    }
}