using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using SynopticumDAL.Contract;

namespace SynopticumDAL
{
    public class UnitOfWork(DbContext _context) : IUnitOfWork
    {
        private IDbContextTransaction _currentTransaction;

        public IDbContextTransaction BeginTransaction()
        {
            lock (_context)
            {
                if (_currentTransaction != null)
                {
                    throw new UnitOfWorkAlreadyInTransactionStateException();
                }

                _currentTransaction = _context.Database.BeginTransaction();
            }
            return _currentTransaction;
        }

        IRepository<TEntity> IUnitOfWork.GetRepository<TEntity>()
        {
            return new Repository<TEntity>(_context.Set<TEntity>());
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
