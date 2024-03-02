using Domain.Interfaces.Repositories;
using Domain.Models.Business;
using Infrastructure.Attributes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [AutoRegisterTransientRepository(typeof(IDepartmentRepository))]
    public class DepartmentRepository(Context _context) : IDepartmentRepository
    {
        public async Task<Guid> AddAsync(Department entity)
        {
            await _context.Departments.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Departments.FirstAsync(x => x.Id == id);
            _context.Departments.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Department> GetAllQuery()
        {
            return _context.Departments.AsNoTracking().Where(x => x.EntityStatus != Domain.Utils.EntityStatus.Delete);
        }

        public IQueryable<Department> GetAllFullQuery()
        {
            return _context.Departments
                .Include(i => i.Users.Where(x => x.User.EntityStatus != Domain.Utils.EntityStatus.Delete))
                .ThenInclude(i => i.User)
                .Include(i => i.Items.Where(x => x.Item.EntityStatus != Domain.Utils.EntityStatus.Delete))
                .ThenInclude(i => i.Item)
                .AsNoTracking()
                .Where(x => x.EntityStatus != Domain.Utils.EntityStatus.Delete);
        }

        public IQueryable<Department> GetAllWithItemQuery()
        {
            return _context.Departments
                .Include(i => i.Items.Where(x => x.Item.EntityStatus != Domain.Utils.EntityStatus.Delete))
                .ThenInclude(i => i.Item)
                .AsNoTracking()
                .Where(x => x.EntityStatus != Domain.Utils.EntityStatus.Delete);    
        }

        public IQueryable<Department> GetAllWithUserQuery()
        {
            return _context.Departments
                .Include(i => i.Users.Where(x => x.User.EntityStatus != Domain.Utils.EntityStatus.Delete))
                .ThenInclude(i => i.User)
                .AsNoTracking()
                .Where(x => x.EntityStatus != Domain.Utils.EntityStatus.Delete);
        }

        public async Task<Department?> GetFullAsync(Guid id)
        {
            return await _context.Departments
                .Include(i => i.Users.Where(x => x.User.EntityStatus != Domain.Utils.EntityStatus.Delete))
                .ThenInclude(i => i.User)
                .Include(i => i.Items.Where(x => x.Item.EntityStatus != Domain.Utils.EntityStatus.Delete))
                .ThenInclude(i => i.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.EntityStatus != Domain.Utils.EntityStatus.Delete);
        }

        public async Task<Guid> Save(Department entity)
        {
            var isExist = (await _context.Departments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id)) != null;

            if (isExist) _context.Departments.Update(entity);
            else _context.Departments.Add(entity);

            await _context.SaveChangesAsync();
            return entity.Id;
        }
    }
}
