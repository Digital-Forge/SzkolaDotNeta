using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Domain.Model
{
    public class ItemToDepartment
    {
        //Relations
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
