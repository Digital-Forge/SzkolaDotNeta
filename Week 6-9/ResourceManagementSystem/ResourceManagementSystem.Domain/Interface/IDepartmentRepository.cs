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
        bool DeleteDepartmentById(Guid id);
        bool UpdateDepartment(Department department);
        bool AddUserToDepartment(AppUser user, Department department);
        bool AddUserToDepartment(string userId, string departmentId);
        bool AddUserToDepartment(string userId, Guid departmentId);
        bool RemoveUserFromDepartment(AppUser user, Department department);
        bool RemoveUserFromDepartment(string userId, string departmentId);
        bool RemoveUserFromDepartment(string userId, Guid departmentId);
        IQueryable<Department> GetDepartmentsList();
        IQueryable<Department> GetDepartmentsListByUser(string userId);
        IQueryable<AppUser> GetUsersListByDepartment(string departmentId);
        IQueryable<AppUser> GetUsersListByDepartment(Guid departmentId);
        Department GetDepartmentById(string id);
        Department GetDepartmentById(Guid id);
        bool AddItemToDepartment(Item item, Department department);
        bool AddItemToDepartment(string itemId, string departmentId);
        bool AddItemToDepartment(Guid itemId, Guid departmentId);
        bool RemoveItemFromDepartment(Item item, Department department);
        bool RemoveItemFromDepartment(string itemId, string departmentId);
        bool RemoveItemFromDepartment(Guid itemId, Guid departmentId);
    }
}
