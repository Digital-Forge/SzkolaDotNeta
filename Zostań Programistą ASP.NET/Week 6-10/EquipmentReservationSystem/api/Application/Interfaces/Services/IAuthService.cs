using Domain.Models;

namespace Application.Interfaces
{
    public partial interface IAuthService
    {
        Task<TokenModel> LoginAsync(LoginModel model);
        Task<TokenModel> RefreshAsync(string refreshToken);
        Task<TokenModel> GenerateTokenAsync(UserData user);
        Task LogoutAsync();
        Task<bool> IsUserAdmin(Guid? userId = null);
        Task<bool> IsAccessToPickUpPoint(Guid? userId = null);
    }
}
