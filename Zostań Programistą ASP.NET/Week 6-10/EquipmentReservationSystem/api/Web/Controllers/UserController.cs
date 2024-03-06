using Application.Constans;
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

        [HttpGet]
        [Authorize(Roles = Constans.Role.Name.Administration)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [HttpDelete]
        [Authorize(Roles = Constans.Role.Name.Administration)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = Constans.Role.Name.Administration)]
        public async Task<IActionResult> GetFull(Guid id)
        {
            return Ok(await _userService.GetFullAsync(id));
        }

        [HttpPut]
        [Authorize(Roles = Constans.Role.Name.Administration)]
        public async Task<IActionResult> Create(IUserService.UserFullModel model)
        {
            return Ok(await _userService.Create(model));
        }

        [HttpPatch]
        [Authorize(Roles = Constans.Role.Name.Administration)]
        public async Task<IActionResult> Update(IUserService.UserFullModel model)
        {
            await _userService.Update(model);
            return Ok();
        }
    }
}
