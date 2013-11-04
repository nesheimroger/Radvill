using System;
using Radvill.Services.DataFactory.Repositories;

namespace Radvill.Services.DataFactory
{
    public interface IDataFactory : IDisposable
    {
        IUserRepository UserRepository { get; set; }
        ICategoryRepository CategoryRepository { get; set; }
        IQuestionRepository QuestionRepository { get; set; }
        IAnswerRepository AnswerRepository { get; set; }
        IPendingQuestionRepository PendingQuestionRepository { get; set; }
        IAdvisorProfileRepository AdvisorProfileRepository { get; set; }

        void Commit();
    }
}