using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController(IUserService _userService) : ControllerBase
    {
        [HttpGet]
        public IActionResult PanelAccess()
        {
            return Ok(_userService.GetPanelAccess());
        }
    }
}
