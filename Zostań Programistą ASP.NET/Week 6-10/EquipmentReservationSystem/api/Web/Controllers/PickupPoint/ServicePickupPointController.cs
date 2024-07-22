using Application.Constans;
using Application.Interfaces;
using Application.Interfaces.Services.PickupPoint;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.PickupPoint
{
    [ApiController]
    [Route("api/PickupPoint/Service/[action]")]
    [Authorize(Roles = $"{Constans.Role.Name.PickupPoint},{Constans.Role.Name.Administration}")]
    public class ServicePickupPointController(IServicePickupPointService _serviceService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> GetServicesItems(IServicePickupPointService.ServiceItemTableOption options)
        {
            return Ok(await _serviceService.GetServiceItemsAsync(options));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateServieceItem(IServicePickupPointService.ChangeStatusServiceItemModel model)
        {
            await _serviceService.UpdateItemAsync(model);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetServiceItemInfo(Guid id)
        {
            return Ok(await _serviceService.GetServiceItemAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> GetAvailableItemInService(IComboBoxApi<Guid>.SearchOption option)
        {
            return Ok(await _serviceService.GetAvailableServiceItemAsync(option));
        }
    }
}
