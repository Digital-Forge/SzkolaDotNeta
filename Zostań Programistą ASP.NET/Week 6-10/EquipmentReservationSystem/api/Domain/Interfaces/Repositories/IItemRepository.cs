using Domain.Interfaces.Repositories.Abstract;
using Domain.Models.Business;
using static Domain.Interfaces.Repositories.IItemRepository;

namespace Domain.Interfaces.Repositories
{
    public interface IItemRepository : IRepository<IItemQuery, Item>, IBaseRepository<ItemInstance>
    {
        Task DeleteAsync(Guid id);
        List<string> GetAllServiceNoteForItem(Guid ItemInstanceId);
        Task<List<string>> GetAllServiceNoteForItemAsync(Guid ItemInstanceId);
        void AddServiceNoteToItem(Guid ItemInstanceId, string note);
        Task AddServiceNoteToItemAsync(Guid ItemInstanceId, string note);

        interface IItemQuery : IRepositoryQueryBuilder<Item>
        {
            IItemQuery IncludeDepartments();
            IItemQuery IncludeInstances();
            IItemQuery IncludeInstancesThenReservations();
            IItemQuery IncludeInstancesThenReservationsThenUsers();
            IItemQuery Where(System.Linq.Expressions.Expression<Func<Item, bool>> predicate);
            Item? GetItemById(Guid id);
            Task<Item?> GetItemByIdAsync(Guid id);
            Item? GetItemByName(string name);
            Task<Item?> GetItemByNameAsync(string name);
            List<Item> GetItems();
            Task<List<Item>> GetItemsAsync();

            Item? GetItemByInstanceId(Guid id);
            Task<Item?> GetItemByInstanceIdAsync(Guid id);
            Item? GetItemBySerialNumber(string serialNumber);
            Task<Item?> GetItemBySerialNumberAsync(string serialNumber);
        }
    }
}
