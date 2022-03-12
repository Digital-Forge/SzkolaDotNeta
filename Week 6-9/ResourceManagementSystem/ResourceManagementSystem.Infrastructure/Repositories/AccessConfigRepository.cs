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
        
        public bool RemoveRoleFromUser(string ruleName, AppUser user)
        {
            try
            {
                var roleId = _context.Roles.Where(x => x.Name.ToLower() == ruleName.ToLower()).Select(y => y.Id).FirstOrDefault();
                if (string.IsNullOrEmpty(roleId)) return false;

                var binding = _context.UserRoles.Where(x => x.UserId == user.Id && x.RoleId == roleId).FirstOrDefault();

                if (binding == null) return false;

                _context.UserRoles.Remove(binding);
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public IQueryable<IdentityRole> GetRolesList()
        {
            return _context.Roles;
        }

        public IQueryable<IdentityRole> GetRoleListByUser(string userId)
        {
            return _context.Roles.Where(
                x => _context.UserRoles.Any(y => y.RoleId == x.Id && y.UserId == userId));
        }
    }
}
