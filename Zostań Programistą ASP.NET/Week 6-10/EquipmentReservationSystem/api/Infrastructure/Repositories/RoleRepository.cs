using Domain.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Attributes;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [AutoRegisterTransientRepository(typeof(IRoleRepository))]
    public class RoleRepository(Context _context) : IRoleRepository
    {
        public async Task AddRoleToUser(IdentityRole<Guid> role, UserData user)
        {
            await AddRoleToUser(role.Id, user.Id);
        }

        public async Task AddRoleToUser(Guid roleId, Guid userId)
        {
            if (_context.UserRoles.AsNoTracking().Any(x => x.UserId == userId && x.RoleId == roleId)) return;
            await _context.UserRoles.AddAsync(new IdentityUserRole<Guid>() 
            {
                RoleId = roleId, 
                UserId = userId 
            });
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckUserHasRole(Guid roleId, Guid? userId = null)
        {
            userId ??= _context.GetContextUser().Id;
            return await _context.UserRoles.AnyAsync(x => x.RoleId == roleId && x.UserId == userId);
        }

        public async Task<bool> CheckUserHasRole(string name, Guid? userId = null)
        {
            var role = await _context.Roles.AsNoTracking().FirstAsync(x => x.NormalizedName == name.ToUpper());
            return await CheckUserHasRole(role.Id, userId);
        }

        public async Task<IEnumerable<IdentityRole<Guid>>> GetAllRoles()
        {
            return await _context.Roles.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<IdentityRole<Guid>>> GetAllUserRoles(Guid userId)
        {
            var userRoles = await _context.UserRoles
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(s => s.RoleId)
                .ToListAsync();

            if ((userRoles?.Count ?? 0) == 0) return new List<IdentityRole<Guid>>();

            return await _context.Roles
                .AsNoTracking()
                .Where(x => userRoles.Contains(x.Id))
                .ToListAsync();
        }

        public Task<IdentityRole<Guid>> GetRole(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityRole<Guid>> GetRole(string name)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveRoleFromUser(IdentityRole<Guid> role, UserData user)
        {
            await RemoveRoleFromUser(role.Id, user.Id);
        }

        public async Task RemoveRoleFromUser(Guid roleId, Guid userId)
        {
            var relstion = await _context.UserRoles.Where(x => x.UserId == userId && x.RoleId == roleId).ToListAsync();
            _context.UserRoles.RemoveRange(relstion);
            await _context.SaveChangesAsync();
        }
    }
}
