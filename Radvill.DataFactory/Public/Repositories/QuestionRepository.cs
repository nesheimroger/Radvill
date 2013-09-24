using System.Collections.Generic;
using Radvill.DataFactory.Internal.Services;
using Radvill.Models.AdviseModels;
using Radvill.Services.DataFactory.Repositories;

namespace Radvill.DataFactory.Public.Repositories
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(IRadvillContext context) : base(context)
        {
        }

    }
}