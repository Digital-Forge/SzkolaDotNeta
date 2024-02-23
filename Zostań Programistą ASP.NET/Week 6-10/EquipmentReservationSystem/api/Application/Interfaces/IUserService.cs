namespace Application.Interfaces
{
    public partial interface IUserService
    {
        UserPanelAccessModel GetPanelAccess();
        Guid CreateUser(CreateUserModel user);
    }
}
