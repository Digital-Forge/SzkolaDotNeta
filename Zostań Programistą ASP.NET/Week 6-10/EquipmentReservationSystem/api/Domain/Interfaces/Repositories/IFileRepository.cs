using Domain.Models.System;

namespace Domain.Interfaces.Repositories
{
    public interface IFileRepository
    {
        Task<Guid> SaveDataFileAsync(DataFile file);
        Task<DataFile?> GetDataFileAsync(Guid id, bool isNoTracer = false);
        Task DeleteDataFileAsync(Guid id);
    }
}
