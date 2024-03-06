using Domain.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Attributes;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [AutoRegisterTransientRepository(typeof(IUserRepository))]
    public partial class UserRepository(Context _context) : IUserRepository
    {
        public UserData GetContextUser()
        {
            return _context.GetContextUser();
        }

        public UserData GetUser(string email)
        {
            return _context.Users.AsNoTracking().FirstOrDefault(x => x.EntityStatus != Domain.Utils.EntityStatus.Delete && x.NormalizedEmail == email.ToUpper());
        }

        public UserData GetUser(Guid id)
        {
            return _context.Users.AsNoTracking().FirstOrDefault(x => x.EntityStatus != Domain.Utils.EntityStatus.Delete && x.Id == id);
        }

        public IEnumerable<IdentityRole<Guid>> GetUserRoles(Guid userId)
        {
            var rolesList = _context.UserRoles.Where(x => x.UserId == userId).Select(s => s.RoleId).ToList();
            if (rolesList.Count == 0) return new List<IdentityRole<Guid>>();
            return _context.Roles.Where(x => rolesList.Contains(x.Id)).ToList();
        }

        public IUserRepository.IUserQuery QueryBuilder(bool onlyActive = false, bool asNoTracking = false, bool allowBuffor = false)
        {
            return new UserQuery(_context, onlyActive, asNoTracking, allowBuffor);
        }

        public Guid Save(UserData entity)
        {
            bool isTracked = _context.ChangeTracker.Entries<UserData>().Any(e => e.Entity == entity);
            if (!isTracked)
            {
                var isExist = _context.Users.AsNoTracking().FirstOrDefault(x => x.Id == entity.Id) != null;
                if (isExist) _context.Users.Update(entity);
                else _context.Users.Add(entity);
            }

            _context.SaveChanges();
            return entity.Id;
        }

        public async Task<Guid> SaveAsync(UserData entity)
        {
            bool isTracked = _context.ChangeTracker.Entries<UserData>().Any(e => e.Entity == entity);
            if (!isTracked)
            {
                var isExist = (await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id)) != null;
                if (isExist) _context.Users.Update(entity);
                else _context.Users.Add(entity);
            }

            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public void Delete(Guid id)
        {
            var entity = _context.Users.First(x => x.Id == id);
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Users.FirstAsync(x => x.Id == id);
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
