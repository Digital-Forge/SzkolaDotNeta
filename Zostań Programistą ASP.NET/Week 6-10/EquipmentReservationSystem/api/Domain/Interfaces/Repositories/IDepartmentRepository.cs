using Domain.Models.Business;

namespace Domain.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        Task<ICollection<Department>> GetAllAsync();
        Task<ICollection<Department>> GetAllWithUserAsync();
        Task<ICollection<Department>> GetAllWithItemAsync();
        Task<ICollection<Department>> GetAllFullAsync();
        Task<Guid> AddAsync(Department entity);
        Task DeleteAsync(Guid id);
        Task<Department?> GetFullAsync(Guid id);
        Task<Guid> Save(Department entity);
    }
}
