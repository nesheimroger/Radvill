using Radvill.DataFactory.Internal.Services;
using Radvill.Models.AdviceModels;
using Radvill.Services.DataFactory.Repositories;

namespace Radvill.DataFactory.Public.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IRadvillContext context) : base(context)
        {
        }
    }
}