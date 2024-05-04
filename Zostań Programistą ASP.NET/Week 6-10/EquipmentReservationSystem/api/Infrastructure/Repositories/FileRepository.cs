using Domain.Interfaces.Repositories;
using Domain.Models.System;
using Infrastructure.Attributes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [AutoRegisterScopedRepository(typeof(IFileRepository))]
    public class FileRepository : IFileRepository
    {
        private readonly Context _context;
        private readonly SemaphoreSlim _semaphore;

        public FileRepository(Context context)
        {
            _context = context;
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public async Task<DataFile?> GetAsync(Guid id)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await _context.Files
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id && x.EntityStatus != Domain.Utils.EntityStatus.Delete);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<Guid> SaveAsync(DataFile file)
        {
            var isExist = (await _context.Files.AsNoTracking().FirstOrDefaultAsync(x => x.Id == file.Id)) != null;

            if (isExist) _context.Files.Update(file);
            else _context.Files.Add(file);

            await _context.SaveChangesAsync();
            _context.Entry(file).State = EntityState.Detached;
            return file.Id;
        }

        public async Task RemoveAsync(Guid id)
        {
            var entity = await _context.Files.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return;

            _context.Files.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
