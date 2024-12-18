using Microsoft.EntityFrameworkCore.Storage;
using SynopticumDAL.Contract;
using SynopticumDAL.Contract.Exceptions;

namespace SynopticumDAL.Services
{
    public class UnitOfWork(SynopticumDbContext _context) : IUnitOfWork
    {
        private IDbContextTransaction? _currentTransaction;

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
