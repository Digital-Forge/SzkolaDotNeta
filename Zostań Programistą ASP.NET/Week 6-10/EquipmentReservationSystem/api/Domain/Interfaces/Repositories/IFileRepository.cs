using Domain.Models.System;

namespace Domain.Interfaces.Repositories
{
    public interface IFileRepository
    {
        Task<Guid> SaveAsync(DataFile file);
        Task<DataFile?> GetAsync(Guid id);
        Task RemoveAsync(Guid id);
    }
}
