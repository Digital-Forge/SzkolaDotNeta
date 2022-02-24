using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagementSystem.Domain.Interface
{
    public interface IUserRepository
    {
        IQueryable<AppUser> UsersList();

        bool UpdateUser(AppUser user);
    }
}
