using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories.Abstract
{
    public class TransactionCommander : IRepositoryTransaction.ITransactionCommander
    {
        private readonly DbContext _context;
        private IDbContextTransaction _transaction;

        public TransactionCommander(DbContext context)
        {
            _context = context;
        }

        ~TransactionCommander()
        {
            try
            {
                _transaction?.Dispose();
            }
            catch (Exception)
            {

            }
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
            _transaction.Dispose();
        }

        public void RollbackTransaction()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }
    }
}
