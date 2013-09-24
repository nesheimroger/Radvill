using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Radvill.Models.UserModels;

namespace Radvill.DataFactory.Internal.Services
{
    public interface IRadvillContext : IDisposable
    {
        IDbSet<User> Users { get; set; }
        IDbSet<AdvisorProfile> AdvisorProfiles { get; set; }

        int SaveChanges();
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entryToUpdate) where TEntity : class;
    }
}