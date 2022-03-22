using ResourceManagementSystem.Application.ViewModel.ExtraViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Application.ViewModel.Departments
{
    public class StatusUsersInDepartmentVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<AddRemoveStatusVM> UsersList { get; set; }
    }
}
