namespace Domain.Models.Business.MiddleTabs
{
    public class ItemToDepartment
    {
        public Guid DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }
    }
}
