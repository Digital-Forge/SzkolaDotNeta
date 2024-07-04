using Application.Constans;
using Application.Interfaces;
using Application.Interfaces.Services.PickupPoint;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.PickupPoint
{
    [ApiController]
    [Route("api/PickupPoint/Item/[action]")]
    [Authorize(Roles = $"{Constans.Role.Name.PickupPoint},{Constans.Role.Name.Administration}")]
    public class ItemPickupPointController(IItemPickupPointService _itemService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> GetItemsInService(IItemPickupPointService.ServiceTableOption option)
        {
            return Ok(await _itemService.GetItemsInServiceAsync(option));
        }
        
        [HttpPost]
        public async Task<IActionResult> GetAvailableItemInService(IComboBoxApi<Guid>.SearchOption option)
        {
            return Ok(await _itemService.GetAvailableItemInServiceAsync(option));
        }
    }
}
