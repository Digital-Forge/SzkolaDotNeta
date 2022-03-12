using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Domain.Model
{
    public class UserToDepartment
    {
        //Relations
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public string DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
