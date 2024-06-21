using Domain.Interfaces.Models;

namespace Domain.Interfaces.Repositories.Abstract
{
    public interface IRepository<Q,E> : IBaseRepository<E>
        where Q : class, IRepositoryQueryBuilder<E>
        where E : class, IAuditableEntity
    {
        Q QueryBuilder(bool onlyActive = false, bool asNoTracking = false, bool allowBuffor = false);
    }
}
