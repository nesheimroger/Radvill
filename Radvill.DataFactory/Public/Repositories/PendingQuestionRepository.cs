using Radvill.DataFactory.Internal.Services;
using Radvill.Models.AdviseModels;
using Radvill.Services.DataFactory.Repositories;

namespace Radvill.DataFactory.Public.Repositories
{
    public class PendingQuestionRepository : GenericRepository<PendingQuestion>, IPendingQuestionRepository
    {
        public PendingQuestionRepository(IRadvillContext context) : base(context)
        {
        }
    }
}