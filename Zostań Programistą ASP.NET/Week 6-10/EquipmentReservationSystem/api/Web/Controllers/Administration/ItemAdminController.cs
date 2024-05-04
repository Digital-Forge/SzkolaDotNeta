using Application.Constans;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Interfaces.IItemAdminService;

namespace Web.Controllers.Administration
{
    [ApiController]
    [Route("api/Admin/Item/[action]")]
    [Authorize(Roles = Constans.Role.Name.Administration)]
    public class ItemAdminController(IItemAdminService _itemService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> GetAll(ItemTableOprtion oprtion)
        {
            return Ok(await _itemService.GetTablAsync(oprtion));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _itemService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _itemService.GetModelAsync(id));
        }

        [HttpPut]
        public async Task<IActionResult> Create(ItemModel model)
        {
            return Ok(await _itemService.CreateAsync(model));
        }

        [HttpPatch]
        public async Task<IActionResult> Update(ItemModel model)
        {
            await _itemService.UpdateAsync(model);
            return Ok();
        }
    }
}
