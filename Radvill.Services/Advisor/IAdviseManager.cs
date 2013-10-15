using System;
using Radvill.Models.AdviseModels;

namespace Radvill.Services.Advisor
{
    public interface IAdviseManager
    {
        bool SubmitQuestion(int userid, int categoryId, string question);
        void PassQuestion(PendingQuestion pendingQuestion);
        void PassQuestionForUser(string email);
        bool StartAnswer(PendingQuestion pending);
        DateTime GetDeadline(PendingQuestion pending);
        bool SubmitAnswer(PendingQuestion pending, string answer);
        void AcceptAnswer(Answer answer);
        bool DeclineAnswer(Answer answer);
    }
}