using Application.Constans;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DepartmentController(IDepartmentService _departmentService) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = Constans.Role.Name.Administration)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _departmentService.GetAllAsync());
        }

        [HttpDelete]
        [Authorize(Roles = Constans.Role.Name.Administration)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _departmentService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = Constans.Role.Name.Administration)]
        public async Task<IActionResult> GetFull(Guid id)
        {
            return Ok(await _departmentService.GetFullAsync(id));
        }

        [HttpPut]
        [Authorize(Roles = Constans.Role.Name.Administration)]
        public async Task<IActionResult> Create(IDepartmentService.DepartmentFullModel model)
        {
            return Ok(await _departmentService.Create(model));
        }

        [HttpPatch]
        [Authorize(Roles = Constans.Role.Name.Administration)]
        public async Task<IActionResult> Update(IDepartmentService.DepartmentFullModel model)
        {
            await _departmentService.Update(model);
            return Ok();
        }
    }
}
