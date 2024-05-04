using Domain.Interfaces.Repositories;
using Domain.Models.Business;
using Infrastructure.Attributes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [AutoRegisterTransientRepository(typeof(IItemRepository))]
    public partial class ItemRepository(Context _context) : IItemRepository
    {
        public IItemRepository.IItemQuery QueryBuilder(bool onlyActive = false, bool asNoTracking = false, bool allowBuffor = false)
        {
            return new ItemQuery(_context, onlyActive, asNoTracking, allowBuffor);
        }

        public Guid Save(Item entity)
        {
            bool isTracked = _context.ChangeTracker.Entries<Item>().Any(e => e.Entity == entity);
            if (!isTracked)
            {
                var isExist = _context.Items.AsNoTracking().FirstOrDefault(x => x.Id == entity.Id) != null;
                if (isExist) _context.Items.Update(entity);
                else _context.Items.Add(entity);
            }

            _context.SaveChanges();
            return entity.Id;
        }

        public async Task<Guid> SaveAsync(Item entity)
        {
            bool isTracked = _context.ChangeTracker.Entries<Item>().Any(e => e.Entity == entity);
            if (!isTracked)
            {
                var isExist = (await _context.Items.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id)) != null;
                if (isExist) _context.Items.Update(entity);
                else _context.Items.Add(entity);
            }

            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public void Delete(Guid id)
        {
            var entity = _context.Items.First(x => x.Id == id);
            _context.Items.Remove(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Items.FirstAsync(x => x.Id == id);
            _context.Items.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
