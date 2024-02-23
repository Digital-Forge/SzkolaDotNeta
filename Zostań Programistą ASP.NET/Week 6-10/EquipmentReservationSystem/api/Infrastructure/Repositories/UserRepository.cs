using Domain.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Attributes;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [AutoRegisterTransientRepository(typeof(IUserRepository))]
    public class UserRepository(Context _context) : IUserRepository
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
            if(rolesList.Count == 0) return new List<IdentityRole<Guid>>();
            return _context.Roles.Where(x => rolesList.Contains(x.Id)).ToList();
        }
    }
}
