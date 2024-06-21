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
        public async Task AddRoleToUserAsync(IdentityRole<Guid> role, UserData user)
        {
            await AddRoleToUserAsync(role.Id, user.Id);
        }

        public async Task AddRoleToUserAsync(Guid roleId, Guid userId)
        {
            if (_context.UserRoles.AsNoTracking().Any(x => x.UserId == userId && x.RoleId == roleId)) return;
            await _context.UserRoles.AddAsync(new IdentityUserRole<Guid>() 
            {
                RoleId = roleId, 
                UserId = userId 
            });
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckUserHasRoleAsync(Guid roleId, Guid? userId = null)
        {
            userId ??= _context.GetContextUser().Id;
            return await _context.UserRoles.AnyAsync(x => x.RoleId == roleId && x.UserId == userId);
        }

        public async Task<bool> CheckUserHasRoleAsync(string name, Guid? userId = null)
        {
            var role = await _context.Roles.AsNoTracking().FirstAsync(x => x.NormalizedName == name.ToUpper());
            return await CheckUserHasRoleAsync(role.Id, userId);
        }

        public async Task<IEnumerable<IdentityRole<Guid>>> GetAllRolesAsync()
        {
            return await _context.Roles.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<IdentityRole<Guid>>> GetAllUserRolesAsync(Guid userId)
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

        public Task<IdentityRole<Guid>> GetRoleAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityRole<Guid>> GetRoleAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveRoleFromUserAsync(IdentityRole<Guid> role, UserData user)
        {
            await RemoveRoleFromUserAsync(role.Id, user.Id);
        }

        public async Task RemoveRoleFromUserAsync(Guid roleId, Guid userId)
        {
            var relstion = await _context.UserRoles.Where(x => x.UserId == userId && x.RoleId == roleId).ToListAsync();
            _context.UserRoles.RemoveRange(relstion);
            await _context.SaveChangesAsync();
        }
    }
}
