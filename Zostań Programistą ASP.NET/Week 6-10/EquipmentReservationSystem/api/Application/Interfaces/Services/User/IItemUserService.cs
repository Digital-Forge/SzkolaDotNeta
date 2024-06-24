namespace Application.Interfaces.Services.User
{
    public partial interface IItemUserService
    {
        Task<IPaginationTable<ItemTableModel>.TableData> GetUserAvailableItemsAsync(ItemTableOption options, Guid? userId = null);
        Task<ItemModel> GetItemAsync(Guid id);
        Task CheckAvailableItemAsync(Guid id);
    }
}
