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
        [Authorize(Roles = Constans.RoleName.Administration)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _departmentService.GetAllAsync());
        }

        [HttpPost]
        [Authorize(Roles = Constans.RoleName.Administration)]
        public async Task<IActionResult> Add(IDepartmentService.DepartmentBaseModel model)
        {
            return Ok(await _departmentService.AddAsync(model));
        }

        [HttpDelete]
        [Authorize(Roles = Constans.RoleName.Administration)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _departmentService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = Constans.RoleName.Administration)]
        public async Task<IActionResult> GetFull(Guid id)
        {
            return Ok(await _departmentService.GetFullAsync(id));
        }

        [HttpPut]
        [Authorize(Roles = Constans.RoleName.Administration)]
        public async Task<IActionResult> Create(IDepartmentService.DepartmentFullModel model)
        {
            return Ok(await _departmentService.Create(model));
        }

        [HttpPatch]
        [Authorize(Roles = Constans.RoleName.Administration)]
        public async Task<IActionResult> Update(IDepartmentService.DepartmentFullModel model)
        {
            await _departmentService.Update(model);
            return Ok();
        }

    }
}
