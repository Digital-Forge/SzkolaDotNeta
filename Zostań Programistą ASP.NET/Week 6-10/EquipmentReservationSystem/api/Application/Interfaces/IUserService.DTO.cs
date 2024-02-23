namespace Application.Interfaces
{
    public partial interface IUserService
    {
        class CreateUserModel
        {
            public string Email { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public IList<Guid> DepartmentIdList { get; set; }
        }
    }
}
