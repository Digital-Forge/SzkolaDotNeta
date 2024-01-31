using Application.Attributes;
using Application.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    [AutoRegisterTransientService(typeof(IAuthorizationService))]
    public partial class AuthorizationService : IAuthorizationService
    {
        private readonly SignInManager<UserData> _signInManager;
        private readonly UserManager<UserData> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthorizationService(SignInManager<UserData> signInManager, UserManager<UserData> userManager, IConfiguration configuration, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = configuration;
        }

        public async Task<string> Login(IAuthorizationService.ILoginModel model)
        {
            if (model == null) return null;
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password)) return null;

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (!result.Succeeded) return null;
            return await GenerateJsonWebToken(model);
        }

        public async Task<string> GenerateJsonWebToken(IAuthorizationService.ILoginModel model)
        {
            var user = _userRepository.GetUser(model.Username);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>() { new Claim("id", user.Id.ToString()) };

            var roles = _userRepository.GetUserRoles(user.Id);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var token = new JwtSecurityToken(_config["JWT:Issuer"], _config["JWT:Issuer"], claims, null, DateTime.Now.AddMinutes(600), credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
            return;
        }
    }
}
