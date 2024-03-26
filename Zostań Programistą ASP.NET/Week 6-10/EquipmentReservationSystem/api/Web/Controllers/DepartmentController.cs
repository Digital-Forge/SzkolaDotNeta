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

        [HttpGet]
        [Authorize(Roles = $"{Constans.Role.Name.PickupPoint},{Constans.Role.Name.Administration}")]
        public async Task<IActionResult> GetAllCombo()
        {
            return Ok(await _departmentService.GetAllComboAsync());
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
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _departmentService.GetModelAsync(id));
        }

        [HttpPut]
        [Authorize(Roles = Constans.Role.Name.Administration)]
        public async Task<IActionResult> Create(IDepartmentService.DepartmentModel model)
        {
            return Ok(await _departmentService.CreateAsync(model));
        }

        [HttpPatch]
        [Authorize(Roles = Constans.Role.Name.Administration)]
        public async Task<IActionResult> Update(IDepartmentService.DepartmentModel model)
        {
            await _departmentService.UpdateAsync(model);
            return Ok();
        }
    }
}
