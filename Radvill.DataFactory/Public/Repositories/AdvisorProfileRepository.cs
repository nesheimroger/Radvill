using Radvill.DataFactory.Internal.Services;
using Radvill.Models.UserModels;
using Radvill.Services.DataFactory.Repositories;

namespace Radvill.DataFactory.Public.Repositories
{
    public class AdvisorProfileRepository : GenericRepository<AdvisorProfile>, IAdvisorProfileRepository
    {
        public AdvisorProfileRepository(IRadvillContext context) : base(context)
        {
        }
    }
}