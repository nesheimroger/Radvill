using System.Data.Entity;
using Radvill.DataFactory.Internal.Services;
using Radvill.Models.AdviceModels;
using Radvill.Models.UserModels;

namespace Radvill.DataFactory.Internal.Components
{
    public class RadvillContext : DbContext, IRadvillContext
    {
        public RadvillContext() : base(Radvill.Configuration.Database.ConnectionStringName)
        {
             
        }

        public IDbSet<User> Users { get; set; }
        public IDbSet<AdvisorProfile> AdvisorProfiles { get; set; }
        public IDbSet<Category> Categories { get; set; }
        public IDbSet<Question> Questions { get; set; }
        public IDbSet<Answer> Answers { get; set; }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }
}