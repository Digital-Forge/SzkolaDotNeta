using ResourceManagementSystem.Domain.Interface;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagementSystem.Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly Context _context;

        public DepartmentRepository(Context context)
        {
            _context = context;
        }

        public string AddDepartment(string name)
        {
            try
            {
                var buff = new Department
                {
                    Id = new Guid(),
                    Name = name
                };
                _context.Departments.Add(buff);
                _context.SaveChanges();
                return buff.Id.ToString();
            }
            catch
            {
                return null;
            }
        }

        public bool AddUserToDepartment(AppUser user, Department department)
        {
            return AddUserToDepartment(user.Id, department.Id);
        }

        public bool AddUserToDepartment(string userId, string departmentId)
        {
            return AddUserToDepartment(userId, new Guid(departmentId));
        }

        public bool DeleteDepartmentByName(string name)
        {
            try
            {
                var buff = _context.Departments.FirstOrDefault(x => x.Name == name);

                if (buff == null) return false;

                _context.Departments.Remove(buff);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteDepartmentById(string id)
        {
            return DeleteDepartmentById(new Guid(id));
        }

        public IQueryable<Department> GetDepartmentsList()
        {
            return _context.Departments;
        }

        public IQueryable<Department> GetDepartmentsListByUser(string userId)
        {
            return _context.Departments.Where(x => x.AppUsers.Any(y => y.AppUserId == userId));
        }

        public bool RemoveUserFromDepartment(AppUser user, Department department)
        {
            return RemoveUserFromDepartment(user.Id, department.Id);
        }

        public bool RemoveUserFromDepartment(string userId, string departmentId)
        {
            return RemoveUserFromDepartment(userId, new Guid(departmentId));
        }

        public bool UpdateDepartment(Department department)
        {
            try
            {
                _context.Departments.Update(department);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Department GetDepartmentById(string id)
        {
            return GetDepartmentById(new Guid(id));
        }

        public IQueryable<AppUser> GetUsersListByDepartment(string departmentId)
        {
            return GetUsersListByDepartment(new Guid(departmentId));
        }

        public bool AddItemToDepartment(Item item, Department department)
        {
            return AddItemToDepartment(item.Id, department.Id);
        }

        public bool AddItemToDepartment(string itemId, string departmentId)
        {
            return AddItemToDepartment(new Guid(itemId), new Guid(departmentId));
        }

        public bool RemoveItemFromDepartment(Item item, Department department)
        {
            return RemoveItemFromDepartment(item.Id, department.Id);
        }

        public bool RemoveItemFromDepartment(string itemId, string departmentId)
        {
            return RemoveItemFromDepartment(new Guid(itemId), new Guid(departmentId));
        }

        public bool DeleteDepartmentById(Guid id)
        {
            try
            {
                var buff = _context.Departments.FirstOrDefault(x => x.Id == id);

                if (buff == null) return false;

                _context.Departments.Remove(buff);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddUserToDepartment(string userId, Guid departmentId)
        {
            try
            {
                _context.UsersToDepartments.Add(new UserToDepartment { AppUserId = userId, DepartmentId = departmentId });
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveUserFromDepartment(string userId, Guid departmentId)
        {
            try
            {
                var buff = _context.UsersToDepartments.FirstOrDefault(x => x.AppUserId == userId && x.DepartmentId == departmentId);

                if (buff == null) return false;

                _context.UsersToDepartments.Remove(buff);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IQueryable<AppUser> GetUsersListByDepartment(Guid departmentId)
        {
            return _context.UsersToDepartments.Where(x => x.DepartmentId == departmentId).Select(y => y.AppUser);
        }

        public Department GetDepartmentById(Guid id)
        {
            return _context.Departments.FirstOrDefault(x => x.Id == id);
        }

        public bool AddItemToDepartment(Guid itemId, Guid departmentId)
        {
            try
            {
                _context.ItemsToDepartments.Add(new ItemToDepartment { ItemId = itemId, DepartmentId = departmentId });
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveItemFromDepartment(Guid itemId, Guid departmentId)
        {
            try
            {
                var buff = _context.ItemsToDepartments.FirstOrDefault(x => x.ItemId == itemId && x.DepartmentId == departmentId);

                if (buff == null) return false;

                _context.ItemsToDepartments.Remove(buff);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
