namespace Domain.Models.Business.MiddleTabs
{
    public class UserToDepartment
    {
        public Guid UserId { get; set; }
        public virtual UserData User { get; set; }
        public Guid DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}
