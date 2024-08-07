namespace Infrastructure.Repositories.Abstract
{
    public partial interface IRepositoryTransaction
    {
        ITransactionCommander Transactions { get; set; }
    }
}
