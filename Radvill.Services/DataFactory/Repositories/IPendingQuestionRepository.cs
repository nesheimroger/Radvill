using System.Collections.Generic;
using Radvill.Models.AdviseModels;

namespace Radvill.Services.DataFactory.Repositories
{
    public interface IPendingQuestionRepository : IRepository<PendingQuestion>
    {
        List<PendingQuestion> GetByQuestionID(int id);
        PendingQuestion GetCurrentByUser(string email);
    }
}