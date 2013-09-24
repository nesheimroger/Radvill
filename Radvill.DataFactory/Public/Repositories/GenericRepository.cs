using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Radvill.DataFactory.Internal.Services;
using Radvill.Services.DataFactory.Repositories;

namespace Radvill.DataFactory.Public.Repositories
{
    public abstract class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class 
    {
        internal IRadvillContext Context;
        internal IDbSet<TEntity> DbSet;

        protected GenericRepository(IRadvillContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();

        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }


            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        public virtual TEntity GetByID(int id)
        {
            return DbSet.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Get();
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }


        public virtual void Delete(int id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}