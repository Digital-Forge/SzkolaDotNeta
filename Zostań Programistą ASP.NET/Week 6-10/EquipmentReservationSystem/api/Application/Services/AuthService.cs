using Application.Attributes;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Models.System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    [AutoRegisterTransientService(typeof(IAuthService))]
    public partial class AuthService : IAuthService
    {
        private readonly SignInManager<UserData> _signInManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _config;
        private readonly ILogService _logger;

        public AuthService(SignInManager<UserData> signInManager, IConfiguration configuration, IUserRepository userRepository, ITokenRepository tokenRepository, IRoleRepository roleRepository, ILogService logService)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _signInManager = signInManager;
            _config = configuration;
            _logger = logService;
        }

        public async Task<IAuthService.TokenModel> LoginAsync(IAuthService.LoginModel model)
        {
            if (model == null) return null;
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password)) return null;
            var user = _userRepository.GetUser(model.Email);
            if (user == null) return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded) return null;
            await _logger.InfoLogAsync($"Login user ({user.UserName})", source: typeof(AuthService).Name);
            return await GenerateTokenAsync(user);
        }

        public async Task<IAuthService.TokenModel> RefreshAsync(string token)
        {
            var refresh = await _tokenRepository.GetTokenAsync(token);
            if (refresh == null || refresh.LifeTime < DateTime.UtcNow) throw new RefreshTokenExpiredException();

            var user = _userRepository.GetUser(refresh.UserId);
            return await GenerateTokenAsync(user);
        }

        public async Task<IAuthService.TokenModel> GenerateTokenAsync(UserData user)
        {
            var timeSource = DateTime.UtcNow;
            return new IAuthService.TokenModel()
            {
                Token = await GenerateAccessTokenAsync(user, timeSource),
                RefreshToken = await GenerateRefreshTokenAsync(user, timeSource),
                Expiry = await GetLifeTimeRefresh(timeSource)
            };
        }

        private async Task<string> GenerateAccessTokenAsync(UserData user, DateTime timeSource)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = _userRepository.GetUserRoles(user.Id);

            var claims = new List<Claim>() 
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim("XID", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var token = new JwtSecurityToken(_config["Token:JWT:Issuer"],
                                             _config["Token:JWT:Audience"],
                                             claims,
                                             null,
                                             timeSource.AddMinutes(double.Parse(_config["Token:LifeTimeToken"])),
                                             credentials);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> GenerateRefreshTokenAsync(UserData user, DateTime timeSource)
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            var refreshToken = Convert.ToBase64String(randomNumber);
            var token = new RefreshToken()
            {
                Token = refreshToken,
                LifeTime = await GetLifeTimeRefresh(timeSource),
                UserId = user.Id
            };
            await _tokenRepository.SaveAsync(token);
            return refreshToken;
        }

        private async Task<DateTime> GetLifeTimeRefresh(DateTime timeSource)
        {
            return timeSource.AddMinutes(double.Parse(_config["Token:LifeTimeRefreshToken"]));
        }

        public async Task LogoutAsync()
        {
            var user = _userRepository.GetContextUser();
            await _logger.InfoLogAsync($"Logout user ({user.UserName})", source: typeof(AuthService).Name);
            await _tokenRepository.RemoveTokenAsync(user.Id);
            //await _signInManager.SignOutAsync();
        }

        public async Task<bool> IsUserAdminAsync(Guid? userId = null)
        {
            return await _roleRepository.CheckUserHasRoleAsync(Constans.Constans.Role.Name.Administration, userId);
        }

        public async Task<bool> IsAccessToPickUpPointAsync(Guid? userId = null)
        {
            if (await IsUserAdminAsync(userId)) return true;
            return await _roleRepository.CheckUserHasRoleAsync(Constans.Constans.Role.Name.PickupPoint, userId);
        }
    }
}
