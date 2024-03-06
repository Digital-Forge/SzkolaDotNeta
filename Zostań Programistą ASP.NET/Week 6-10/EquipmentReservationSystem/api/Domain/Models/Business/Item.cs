using Domain.Models.Business.MiddleTabs;

namespace Domain.Models.Business
{
    public class Item : AuditableEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<Guid> Images { get; set; } = new List<Guid>();
        public List<Guid> AddictionFiles { get; set; } = new List<Guid>();

        //Relations
        public virtual List<ItemInstance> Instances { get; set; } = new List<ItemInstance>();
        public virtual List<ItemToDepartment> Departments { get; set; } = new List<ItemToDepartment>();
    }
}
