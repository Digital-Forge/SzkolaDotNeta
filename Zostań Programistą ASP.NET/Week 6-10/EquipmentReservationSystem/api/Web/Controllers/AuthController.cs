using Application.Constans;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(IAuthService.LoginModel login)
        {
            var token = await _authService.LoginAsync(login);
            if (token == null) return Unauthorized();
            return Ok(token);
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken(string token)
        {
            if (token == null) return BadRequest();
            var newToken = await _authService.RefreshAsync(token);
            return Ok(newToken);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CheckAuth()
        {
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = Constans.RoleName.Administration)]
        public async Task<IActionResult> CheckAdminAuth()
        {
            var check = await _authService.IsUserAdmin();
            return check ? Ok() : Forbid();
        }

        [HttpGet]
        [Authorize(Roles = $"{Constans.RoleName.PickUpPoint},{Constans.RoleName.Administration}")]
        public async Task<IActionResult> CheckPickUpPointAuth()
        {
            var check = await _authService.IsAccessToPickUpPoint();
            return check ? Ok() : Forbid();
        }
    }
}
