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
            var entity = _context.Items.Include(i => i.Instances).FirstOrDefault(x => x.Id == id)
                      ?? _context.ItemInstances.Include(i => i.Item).First(x => x.Id == id).Item;

            foreach (var instance in entity.Instances)
            {
                _context.ItemInstances.Remove(instance);
            }

            _context.Items.Remove(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = (await _context.Items.Include(i => i.Instances).FirstOrDefaultAsync(x => x.Id == id))
                      ?? (await _context.ItemInstances.Include(i => i.Item).FirstAsync(x => x.Id == id)).Item;

            foreach (var instance in entity.Instances)
            {
                _context.ItemInstances.Remove(instance);
            }

            _context.Items.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Guid Save(ItemInstance entity)
        {
            bool isTracked = _context.ChangeTracker.Entries<ItemInstance>().Any(e => e.Entity == entity);
            if (!isTracked)
            {
                var isExist = (_context.ItemInstances.AsNoTracking().FirstOrDefault(x => x.Id == entity.Id)) != null;
                if (isExist) _context.ItemInstances.Update(entity);
                else _context.ItemInstances.Add(entity);
            }

            _context.SaveChanges();
            return entity.Id;
        }

        public async Task<Guid> SaveAsync(ItemInstance entity)
        {
            bool isTracked = _context.ChangeTracker.Entries<ItemInstance>().Any(e => e.Entity == entity);
            if (!isTracked)
            {
                var isExist = (await _context.ItemInstances.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id)) != null;
                if (isExist) _context.ItemInstances.Update(entity);
                else _context.ItemInstances.Add(entity);
            }

            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public List<string> GetAllServiceNoteForItem(Guid ItemInstanceId)
        {
            return _context.ServiceNotes
                .Where(x => x.ItemInstanceId == ItemInstanceId)
                .OrderByDescending(o => o.CreateTime)
                .Select(s => s.Note)
                .ToList();
        }

        public async Task<List<string>> GetAllServiceNoteForItemAsync(Guid ItemInstanceId)
        {
            return await _context.ServiceNotes
                .Where(x => x.ItemInstanceId == ItemInstanceId)
                .OrderByDescending(o => o.CreateTime)
                .Select(s => s.Note)
                .ToListAsync();
        }

        public void AddServiceNoteToItem(Guid ItemInstanceId, string note)
        {
            _context.ServiceNotes.Add(new ServiceNote { ItemInstanceId = ItemInstanceId, Note = note });
            _context.SaveChanges();
        }

        public async Task AddServiceNoteToItemAsync(Guid ItemInstanceId, string note)
        {
            await _context.ServiceNotes.AddAsync(new ServiceNote { ItemInstanceId = ItemInstanceId, Note = note });
            await _context.SaveChangesAsync();
        }
    }
}
