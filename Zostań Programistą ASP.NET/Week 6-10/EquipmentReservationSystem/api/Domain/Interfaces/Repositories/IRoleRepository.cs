using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task AddRoleToUser(IdentityRole<Guid> role, UserData user);
        Task AddRoleToUserAsync(Guid roleId, Guid userId);
        Task RemoveRoleFromUser(IdentityRole<Guid> role, UserData user);
        Task RemoveRoleFromUser(Guid roleId, Guid userId);
        Task<IEnumerable<IdentityRole<Guid>>> GetAllRoles();
        Task<IEnumerable<IdentityRole<Guid>>> GetAllUserRoles(Guid userId);
        Task<IdentityRole<Guid>> GetRole(Guid id);
        Task<IdentityRole<Guid>> GetRole(string name);
        Task<bool> CheckUserHasRole(Guid roleId, Guid? userId = null);
        Task<bool> CheckUserHasRole(string name, Guid? userId = null);
    }
}
