using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        UserData GetUser(string username);
        UserData GetUser(Guid id);
        IEnumerable<IdentityRole<Guid>> GetUserRoles(Guid userId);
    }
}
