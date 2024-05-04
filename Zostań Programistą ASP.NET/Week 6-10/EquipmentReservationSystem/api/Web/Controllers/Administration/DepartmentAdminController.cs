using Application.Constans;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Administration
{
    [ApiController]
    [Route("api/Admin/Department/[action]")]
    [Authorize(Roles = Constans.Role.Name.Administration)]
    public class DepartmentAdminController(IDepartmentAdminService _departmentService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> GetAll(IPaginationTable<IDepartmentAdminService.DepartmentTableModel>.TableOptions options)
        {
            return Ok(await _departmentService.GetTablAsync(options));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCombo()
        {
            return Ok(await _departmentService.GetAllComboAsync());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _departmentService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _departmentService.GetModelAsync(id));
        }

        [HttpPut]
        public async Task<IActionResult> Create(IDepartmentAdminService.DepartmentModel model)
        {
            return Ok(await _departmentService.CreateAsync(model));
        }

        [HttpPatch]
        public async Task<IActionResult> Update(IDepartmentAdminService.DepartmentModel model)
        {
            await _departmentService.UpdateAsync(model);
            return Ok();
        }
    }
}
