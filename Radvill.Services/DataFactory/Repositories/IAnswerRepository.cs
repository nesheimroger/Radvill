using System.Collections.Generic;
using Radvill.Models.AdviseModels;

namespace Radvill.Services.DataFactory.Repositories
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        List<Answer> GetByQuestionId(int id);
    }
}