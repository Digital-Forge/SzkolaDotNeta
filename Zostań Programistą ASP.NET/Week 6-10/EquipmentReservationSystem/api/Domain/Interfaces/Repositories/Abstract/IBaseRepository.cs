using Domain.Interfaces.Models;

namespace Domain.Interfaces.Repositories.Abstract
{
    public interface IBaseRepository<E> where E : class, IAuditableEntity
    {
        Guid Save(E entity);
        Task<Guid> SaveAsync(E entity);
        void Delete(Guid id);
        Task DeleteAsync(Guid id);
    }
}
