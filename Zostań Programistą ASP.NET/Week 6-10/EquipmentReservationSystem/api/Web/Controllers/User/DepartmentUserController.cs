using Application.Interfaces;
using Application.Interfaces.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.User
{
    [Authorize]
    [ApiController]
    [Route("api/Department/[action]")]
    public class DepartmentUserController(IDepartmentUserService _departmentUserService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> GetUserDepartments(IComboBoxApi<Guid>.SearchOption search)
        {
            return Ok(await _departmentUserService.GetUserAvailableDepartmentsAsync(search));
        }
    }
}
