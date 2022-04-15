using Microsoft.AspNetCore.Identity;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagementSystem.Domain.Interface
{
    public interface IAccessConfigRepository
    {
        bool AddRole(string name); 
        bool AddRoleToUser(string ruleName, AppUser user);
        bool RemoveRoleFromUser(string ruleName, AppUser user);

        IQueryable<IdentityRole> GetRolesList();
        IQueryable<IdentityRole> GetRoleListByUser(string userId);
    }
}
