using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Models.Business;
using Infrastructure.Attributes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [AutoRegisterTransientRepository(typeof(IDepartmentRepository))]
    public partial class DepartmentRepository(Context _context) : IDepartmentRepository
    {
        public IDepartmentRepository.IDepartmentQuery QueryBuilder(bool onlyActive = false, bool asNoTracking = false, bool allowBuffor = false)
        {
            return new DepartmentQuery(_context, onlyActive, asNoTracking, allowBuffor);
        }

        public Guid Save(Department entity)
        {
            bool isTracked = _context.ChangeTracker.Entries<Department>().Any(e => e.Entity == entity);
            if (!isTracked)
            {
                var isExist = _context.Departments.AsNoTracking().FirstOrDefault(x => x.Id == entity.Id) != null;
                if (isExist) _context.Departments.Update(entity);
                else _context.Departments.Add(entity);
            }

            _context.SaveChanges();
            return entity.Id;
        }

        public async Task<Guid> SaveAsync(Department entity)
        {
            bool isTracked = _context.ChangeTracker.Entries<Department>().Any(e => e.Entity == entity);
            if (!isTracked)
            {
                var isExist = (await _context.Departments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id)) != null;
                if (isExist) _context.Departments.Update(entity);
                else _context.Departments.Add(entity);
            }

            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public void Delete(Guid id)
        {
            var entity = _context.Departments.First(x => x.Id == id);
            _context.Departments.Remove(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Departments.FirstAsync(x => x.Id == id);
            _context.Departments.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
