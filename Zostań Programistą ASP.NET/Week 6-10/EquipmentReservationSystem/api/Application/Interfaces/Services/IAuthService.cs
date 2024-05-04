using Domain.Models;

namespace Application.Interfaces
{
    public partial interface IAuthService
    {
        Task<TokenModel> LoginAsync(LoginModel model);
        Task<TokenModel> RefreshAsync(string refreshToken);
        Task<TokenModel> GenerateTokenAsync(UserData user);
        Task LogoutAsync();
        Task<bool> IsUserAdminAsync(Guid? userId = null);
        Task<bool> IsAccessToPickUpPointAsync(Guid? userId = null);
    }
}
