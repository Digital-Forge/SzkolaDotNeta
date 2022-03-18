using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagementSystem.Domain.Interface
{
    public interface IDepartmentRepository
    {
        string AddDepartment(string name);
        bool DeleteDepartmentByName(string name);
        bool DeleteDepartmentById(string id);
        bool UpdateDepartment(Department department);
        bool AddUserToDepartment(AppUser user, Department department);
        bool AddUserToDepartment(string userId, string departmentId);
        bool RemoveUserFromDepartment(AppUser user, Department department);
        bool RemoveUserFromDepartment(string userId, string departmentId);
        IQueryable<Department> GetDepartmentsList();
        IQueryable<Department> GetDepartmentsListByUser(string userId);
        IQueryable<AppUser> GetUsersListByDepartment(string departmentId);
        Department GetDepartmentById(string id);
        bool AddItemToDepartment(Item item, Department department);
        bool AddItemToDepartment(string itemId, string departmentId);
        bool RemoveItemFromDepartment(Item item, Department department);
        bool RemoveItemFromDepartment(string itemId, string departmentId);
    }
}
