using static Application.Interfaces.IItemAdminService;

namespace Application.Interfaces
{
    public partial interface IItemAdminService : IPaginationTable<ItemTableModel>
    {
        public Task<Guid> CreateAsync(ItemModel model);
        public Task<ItemModel> GetModelAsync(Guid id);
        public Task UpdateAsync(ItemModel model);
        public Task DeleteAsync(Guid id);
    }
}
