using Domain.Models.System;

namespace Domain.Interfaces.Repositories
{
    public interface ITokenRepository
    {
        Task<RefreshToken?> GetUserTokenAsync(Guid userId);
        Task<RefreshToken?> GetTokenAsync(string token);
        Task SaveTokenAsync(RefreshToken token);
        Task RemoveTokenAsync(RefreshToken token);
        Task RemoveTokenAsync(Guid userId);
        Task RemoveTokenAsync(string token);
    }
}
