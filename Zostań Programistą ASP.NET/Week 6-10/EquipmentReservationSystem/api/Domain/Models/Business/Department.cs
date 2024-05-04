using Domain.Models.Business.MiddleTabs;

namespace Domain.Models.Business
{
    public class Department : AuditableEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        //Relations
        public virtual List<ItemToDepartment> Items { get; set; } = new List<ItemToDepartment>();
        public virtual List<UserToDepartment> Users { get; set; } = new List<UserToDepartment>();
    }
}
