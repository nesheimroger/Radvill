using Radvill.Models.AdviseModels;

namespace Radvill.Services.Events
{
    public delegate void QuestionAssignedEventHandler(PendingQuestion pendingQuestion);
    public delegate void QuestionPassedEventHandler(PendingQuestion pendingQuestion);
    public delegate void AnswerStartedEventHandler(PendingQuestion pendingQuestion);
    public delegate void AnswerSubmittedEventHandler(Answer answer);
    
    public interface IEventSubscriber
    {
        event QuestionAssignedEventHandler Question_Submitted;
        event QuestionPassedEventHandler Question_Passed;
        event AnswerStartedEventHandler Answer_Started;
        event AnswerSubmittedEventHandler Answer_Submitted;
    }
}