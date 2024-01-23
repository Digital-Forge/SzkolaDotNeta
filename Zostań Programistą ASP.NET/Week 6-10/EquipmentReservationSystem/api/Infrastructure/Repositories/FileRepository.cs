using Domain.Interfaces.Repositories;
using Domain.Models.System;
using Infrastructure.Attributes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [AutoRegisterSingletonRepository(typeof(IFileRepository))]
    public class FileRepository(Context _context) : IFileRepository
    {
        public async Task<DataFile?> GetDataFileAsync(Guid id, bool asNoTracking = false)
        {
            return asNoTracking 
                ? await _context.Files.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.EntityStatus != Domain.Utils.EntityStatus.Delete) 
                : await _context.Files.FirstOrDefaultAsync(x => x.Id == id && x.EntityStatus != Domain.Utils.EntityStatus.Delete);
        }

        public async Task<Guid> SaveDataFileAsync(DataFile file)
        {
            var isExist = (await _context.Files.AsNoTracking().FirstOrDefaultAsync(x => x.Id == file.Id)) != null;

            if (isExist)
                _context.Files.Update(file);
            else
                _context.Files.Add(file);

            await _context.SaveChangesAsync();
            return file.Id;
        }

        public async Task DeleteDataFileAsync(Guid id)
        {
            var entity = await _context.Files.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return;

            _context.Files.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
