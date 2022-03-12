using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Application.ViewModel.Users
{
    public class UserOfListVM
    {
        public string Index { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int CountOfDepartments { get; set; }
        public int CountOfResources { get; set; }
        public bool isActive { get; set; }
    }
}
