using ResourceManagementSystem.Domain.Interface;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagementSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }

        public bool UpdateUser(AppUser user)
        {
            try
            {
                _context.AppUsers.Update(user);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public IQueryable<AppUser> UsersList()
        {
            return _context.AppUsers;
        }
    }
}
