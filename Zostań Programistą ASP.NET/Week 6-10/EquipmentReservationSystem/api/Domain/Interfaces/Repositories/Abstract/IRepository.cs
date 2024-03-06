using Domain.Interfaces.Models;

namespace Domain.Interfaces.Repositories.Abstract
{
    public interface IRepository<Q,E> 
        where Q : class, IRepositoryQuerybuilder<E>
        where E : class, IAuditableEntity
    {
        Q QueryBuilder(bool onlyActive = false, bool asNoTracking = false, bool allowBuffor = false);
        Guid Save(E entity);
        Task<Guid> SaveAsync(E entity);
        void Delete(Guid id);
        Task DeleteAsync(Guid id);
    }
}
