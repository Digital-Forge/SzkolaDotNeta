using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task AddRoleToUserAsync(IdentityRole<Guid> role, UserData user);
        Task AddRoleToUserAsync(Guid roleId, Guid userId);
        Task RemoveRoleFromUserAsync(IdentityRole<Guid> role, UserData user);
        Task RemoveRoleFromUserAsync(Guid roleId, Guid userId);
        Task<IEnumerable<IdentityRole<Guid>>> GetAllRolesAsync();
        Task<IEnumerable<IdentityRole<Guid>>> GetAllUserRolesAsync(Guid userId);
        Task<IdentityRole<Guid>> GetRoleAsync(Guid id);
        Task<IdentityRole<Guid>> GetRoleAsync(string name);
        Task<bool> CheckUserHasRoleAsync(Guid roleId, Guid? userId = null);
        Task<bool> CheckUserHasRoleAsync(string name, Guid? userId = null);
    }
}
