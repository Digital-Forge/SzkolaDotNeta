using Domain.Interfaces.Repositories;
using Domain.Models.System;
using Infrastructure.Attributes;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [AutoRegisterTransientRepository(typeof(ITokenRepository))]
    public class TokenRepository(Context _context) : ITokenRepository
    {
        public async Task SaveAsync(RefreshToken token)
        {
            var isExist = await _context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == token.UserId);

            if (isExist != null)
            {
                token.Id = isExist.Id;
                _context.RefreshTokens.Update(token);
            }
            else _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetTokenAsync(string token)
        {
            return await _context.RefreshTokens.AsNoTracking().SingleOrDefaultAsync(x => x.Token == token);
        }

        public async Task<RefreshToken?> GetUserTokenAsync(Guid userId)
        {
            return await _context.RefreshTokens.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task RemoveTokenAsync(RefreshToken token)
        {
            _context.RefreshTokens.Remove(token);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTokenAsync(Guid userId)
        {
            var token = await _context.RefreshTokens.FirstAsync(x => x.UserId == userId);
            await RemoveTokenAsync(token);
        }

        public async Task RemoveTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens.FirstAsync(x => x.Token == refreshToken);
            await RemoveTokenAsync(token);
        }
    }
}
