using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ResourceManagementSystem.Domain.Model
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        //Relations
        public virtual ICollection<UserToDepartment> AppUsers { get; set; }
        public virtual ICollection<ItemToDepartment> Items { get; set; }
    }
}
