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
                    Name = name
                };
                _context.Departments.Add(buff);
                _context.SaveChanges();
                return buff.Id;
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

        public IQueryable<Department> GetDepartmentsList()
        {
            return _context.Departments;
        }

        public IQueryable<Department> GetDepartmentsListByUser(string userId)
        {
            return _context.Departments.Where(x => x.AppUsers.Any(y => y.AppUserId == userId));
        }

        public bool RemoveUserToDepartment(AppUser user, Department department)
        {
            return RemoveUserToDepartment(user.Id, department.Id);
        }

        public bool RemoveUserToDepartment(string userId, string departmentId)
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
    }
}
