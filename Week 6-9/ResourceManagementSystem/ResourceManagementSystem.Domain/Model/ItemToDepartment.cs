using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Domain.Model
{
    public class ItemToDepartment
    {
        //Relations
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
