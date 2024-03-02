using Domain.Models.Business;

namespace Domain.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        IQueryable<Department> GetAllQuery();
        IQueryable<Department> GetAllWithUserQuery();
        IQueryable<Department> GetAllWithItemQuery();
        IQueryable<Department> GetAllFullQuery();
        Task<Guid> AddAsync(Department entity);
        Task DeleteAsync(Guid id);
        Task<Department?> GetFullAsync(Guid id);
        Task<Guid> Save(Department entity);
    }
}
