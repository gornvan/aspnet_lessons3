using Microsoft.EntityFrameworkCore.Storage;
using SynopticumModel.Contract;

namespace SynopticumDAL.Contract
{
    public interface IUnitOfWork
    {
        IDbContextTransaction BeginTransaction();

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;

        Task<int> SaveChangesAsync();
    }
}
