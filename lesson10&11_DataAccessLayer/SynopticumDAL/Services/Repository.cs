using Microsoft.EntityFrameworkCore;
using SynopticumDAL.Contract;
using SynopticumModel.Contract;

namespace SynopticumDAL.Services
{
    public class Repository<TEntity>
        (DbSet<TEntity> _dbSet)
        : IRepository<TEntity> where TEntity : class, IEntity
    {
        public IQueryable<TEntity> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<TEntity> AsReadOnlyQueryable()
        {
            return _dbSet.AsQueryable().AsNoTracking();
        }

        public TEntity Create(TEntity entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public TEntity Delete(TEntity entity)
        {
            return _dbSet.Remove(entity).Entity;
        }
    }
}
