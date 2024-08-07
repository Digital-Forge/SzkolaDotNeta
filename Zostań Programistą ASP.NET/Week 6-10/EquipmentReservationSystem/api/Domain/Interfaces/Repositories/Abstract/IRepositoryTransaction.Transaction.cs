namespace Infrastructure.Repositories.Abstract
{
    public partial interface IRepositoryTransaction
    {
        interface ITransactionCommander
        {
            public void BeginTransaction();
            public void CommitTransaction();
            public void RollbackTransaction();
            public Task BeginTransactionAsync();
            public Task CommitTransactionAsync();
            public Task RollbackTransactionAsync();
        }
    }
}
