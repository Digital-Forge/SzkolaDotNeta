namespace Application.Interfaces
{
    public partial interface IDepartmentService
    {
        public Task<ICollection<DepartmentBaseModel>> GetAllAsync();
        public Task<Guid> AddAsync(DepartmentBaseModel model);
        public Task DeleteAsync(Guid id);
        public Task<DepartmentFullModel> GetFullAsync(Guid id);
        public Task<Guid> Create(DepartmentFullModel model);
        public Task Update(DepartmentFullModel model);
    }
}
