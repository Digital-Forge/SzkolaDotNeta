using Application.Constans;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.DTO;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController(IAuthService _authService, IUserAdminService _userService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(IAuthService.LoginModel login)
        {
            var token = await _authService.LoginAsync(login);
            if (token == null) return Unauthorized();
            return Ok(token);
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken(SingleData<string> token)
        {
            if (token?.Data == null) return BadRequest();
            var newToken = await _authService.RefreshAsync(token.Data);
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
        [Authorize(Roles = Constans.Role.Name.Administration)]
        public async Task<IActionResult> CheckAdminAuth()
        {
            var check = await _authService.IsUserAdminAsync();
            return check ? Ok() : Forbid();
        }

        [HttpGet]
        [Authorize(Roles = $"{Constans.Role.Name.PickupPoint},{Constans.Role.Name.Administration}")]
        public async Task<IActionResult> CheckPickUpPointAuth()
        {
            var check = await _authService.IsAccessToPickUpPointAsync();
            return check ? Ok() : Forbid();
        }

        [HttpGet]
        [Authorize]
        public IActionResult PanelAccess()
        {
            return Ok(_userService.GetPanelAccess());
        }
    }
}
