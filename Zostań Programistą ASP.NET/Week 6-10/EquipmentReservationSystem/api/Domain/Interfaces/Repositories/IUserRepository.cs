using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        UserData GetContextUser();
        UserData GetUser(string email);
        UserData GetUser(Guid id);
        IEnumerable<IdentityRole<Guid>> GetUserRoles(Guid userId);
    }
}
