using Domain.Models.System;

namespace Domain.Interfaces.Repositories
{
    public interface IFileRepository
    {
        Task<Guid> SaveDataFileAsync(DataFile file);
        Task<DataFile?> GetDataFileAsync(Guid id);
        Task RemoveDataFileAsync(Guid id);
    }
}
