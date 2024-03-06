using Domain.Interfaces.Models;

namespace Domain.Interfaces.Repositories.Abstract
{
    public interface IRepositoryQuerybuilder<E> where E : class, IAuditableEntity
    {
        bool AsNoTracking { get; }
        bool OnlyActive { get; }
        bool AllowBuffer { get; }
        IQueryable<E> Query {  get; }
    }
}
