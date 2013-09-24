﻿using System.Data.Entity;
using Radvill.DataFactory.Internal.Services;
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

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }
}