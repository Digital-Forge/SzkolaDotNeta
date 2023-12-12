namespace Domain.Models.Business.MiddleTabs
{
    public class UserToDepartment
    {
        public Guid UserId { get; set; }
        public UserData User { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
