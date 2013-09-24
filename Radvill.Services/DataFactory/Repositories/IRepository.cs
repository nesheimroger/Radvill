using System.Collections.Generic;

namespace Radvill.Services.DataFactory.Repositories
{
    public interface IRepository<TEntity>
    {
        TEntity GetByID(int id);

        IEnumerable<TEntity> GetAll();

        void Insert(TEntity entity);

        void Delete(int id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);
    }
}