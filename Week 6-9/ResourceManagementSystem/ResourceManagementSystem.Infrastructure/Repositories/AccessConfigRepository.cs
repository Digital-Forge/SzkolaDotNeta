using Microsoft.AspNetCore.Identity;
using ResourceManagementSystem.Domain.Interface;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagementSystem.Infrastructure.Repositories
{
    public class AccessConfigRepository : IAccessConfigRepository
    {
        readonly Context _context;

        public AccessConfigRepository(Context context)
        {
            _context = context;
        }

        public bool AddRole(string name)
        {
            try
            {
                _context.Roles.Add(new IdentityRole(name));
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool AddRoleToUser(string ruleName, AppUser user)
        {
            try
            {
                var roleId = _context.Roles.Where(x => x.Name.ToLower() == ruleName.ToLower()).Select(y => y.Id).FirstOrDefault();
                if (string.IsNullOrEmpty(roleId)) return false;

                _context.UserRoles.Add(new IdentityUserRole<string>() 
                { 
                    RoleId = roleId,
                    UserId = user.Id
                });
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
