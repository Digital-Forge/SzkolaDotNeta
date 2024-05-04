using Domain.Interfaces.Repositories.Abstract;
using Domain.Models.Business;
using static Domain.Interfaces.Repositories.IItemRepository;

namespace Domain.Interfaces.Repositories
{
    public interface IItemRepository : IRepository<IItemQuery, Item>
    {
        interface IItemQuery : IRepositoryQuerybuilder<Item>
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
        }
    }
}
