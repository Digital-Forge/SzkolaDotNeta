namespace Application.Interfaces
{
    public partial interface IUserService
    {
        public Guid CreateUser(CreateUserModel user);
    }
}
