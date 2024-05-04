using static Application.Interfaces.IDepartmentAdminService;

namespace Application.Interfaces
{
    public partial interface IDepartmentAdminService : IPaginationTable<DepartmentTableModel>
    {
        public Task<ICollection<DepartmentTableModel>> GetAllAsync();
        public Task<ICollection<DepartmentComboModel>> GetAllComboAsync();
        public Task DeleteAsync(Guid id);
        public Task<DepartmentModel> GetModelAsync(Guid id);
        public Task<Guid> CreateAsync(DepartmentModel model);
        public Task UpdateAsync(DepartmentModel model);
    }
}
