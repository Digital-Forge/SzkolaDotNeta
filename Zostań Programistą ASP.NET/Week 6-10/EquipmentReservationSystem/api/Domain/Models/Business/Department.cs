using Domain.Models.Business.MiddleTabs;

namespace Domain.Models.Business
{
    public class Department : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        //Relations
        public virtual ICollection<ItemToDepartment> Items { get; set; }
        public virtual ICollection<UserToDepartment> Users { get; set; }
    }
}
