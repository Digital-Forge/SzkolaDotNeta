using ResourceManagementSystem.Application.ViewModel.Departments;
using ResourceManagementSystem.Application.ViewModel.ExtraViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Application.Interfaces
{
    public interface IDepartmentModerateService
    {
        List<DepartmentOfListVM> GetDepartmentsList();
        int CreateDepartment(CreateDepartmentVM input);
        DetailsEditDepartmentVM GetDetailsEdit(string id);
        bool Update(DetailsEditDepartmentVM input);
        bool Delete(string id);
        StatusUsersInDepartmentVM GetListToAddUsers(string departmentId);
        void UpdateUsersInDepartment(StatusUsersInDepartmentVM input);
    }
}
