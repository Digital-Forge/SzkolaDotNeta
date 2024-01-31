namespace Application.Interfaces
{
    public partial interface IAuthorizationService
    {
        Task<string> Login(ILoginModel model);
        Task<string> GenerateJsonWebToken(ILoginModel model);
        Task Logout();
    }
}
