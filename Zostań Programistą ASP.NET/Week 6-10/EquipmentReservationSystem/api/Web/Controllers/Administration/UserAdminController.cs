using Application.Constans;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.DTO;

namespace Web.Controllers.Administration
{
    [ApiController]
    [Route("api/Admin/User/[action]")]
    [Authorize(Roles = Constans.Role.Name.Administration)]
    public class UserAdminController(IUserAdminService _userService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> GetAll(IPaginationTable<IUserAdminService.UserTableModel>.TableOptions options)
        {
            return Ok(await _userService.GetTablAsync(options));
        }

        [HttpPost]
        public async Task<IActionResult> GetAllCombo(IUserAdminService.UserComboParams filter)
        {
            return Ok(await _userService.GetAllComboAsync(filter));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetFull(Guid id)
        {
            return Ok(await _userService.GetFullAsync(id));
        }

        [HttpPut]
        public async Task<IActionResult> Create(IUserAdminService.UserModel model)
        {
            return Ok(await _userService.Create(model));
        }

        [HttpPatch]
        public async Task<IActionResult> Update(IUserAdminService.UserModel model)
        {
            await _userService.Update(model);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CheckUserEmailUnique(IUserAdminService.CheckUniqueModel data)
        {
            return Ok(await _userService.CheckUserEmailUnique(data));
        }

        [HttpPost]
        public async Task<IActionResult> CheckUsernameUnique(IUserAdminService.CheckUniqueModel data)
        {
            return Ok(await _userService.CheckUsernameUnique(data));
        }
    }
}
