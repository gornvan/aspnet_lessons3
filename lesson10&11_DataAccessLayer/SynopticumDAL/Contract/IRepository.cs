using SynopticumModel.Contract;

namespace SynopticumDAL.Contract
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        TEntity Create(TEntity entity);

        /// <summary>
        /// The entity must be FOUND in the DB first
        /// </summary>
        TEntity Delete(TEntity entity);

        /// <summary>
        /// Query for Updatable (Tracked) entities
        /// </summary>
        IQueryable<TEntity> AsQueryable();

        /// <summary>
        /// Query for read-only (Untracked) entities
        /// </summary>
        IQueryable<TEntity> AsReadOnlyQueryable();
    }
}
