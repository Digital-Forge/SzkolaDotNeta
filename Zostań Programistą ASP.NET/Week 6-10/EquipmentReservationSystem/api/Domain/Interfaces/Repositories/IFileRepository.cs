using Domain.Models.System;

namespace Domain.Interfaces.Repositories
{
    public interface IFileRepository
    {
        public bool SystemHostedServiceMode { get; set; }

        Task<Guid> SaveAsync(DataFile file);
        Task<DataFile?> GetAsync(Guid id);
        DataFile? Get(Guid id);
        Task RemoveAsync(Guid id);
        void Remove(Guid id);
        IQueryable<DataFile> GetTemporaryFiles();
    }
}
