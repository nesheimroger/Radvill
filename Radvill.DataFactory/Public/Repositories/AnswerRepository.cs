using Radvill.DataFactory.Internal.Services;
using Radvill.Models.AdviseModels;
using Radvill.Services.DataFactory.Repositories;

namespace Radvill.DataFactory.Public.Repositories
{
    public class AnswerRepository : GenericRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(IRadvillContext context) : base(context)
        {
        }
    }
}