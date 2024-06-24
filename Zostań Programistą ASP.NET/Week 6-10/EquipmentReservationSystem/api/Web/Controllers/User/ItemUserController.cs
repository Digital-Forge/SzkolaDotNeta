using Application.Interfaces.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Interfaces.Services.User.IItemUserService;

namespace Web.Controllers.User
{
    [Authorize]
    [ApiController]
    [Route("api/Item/[action]")]
    public class ItemUserController(IItemUserService _itemUserService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> GetAvailableItems(ItemTableOption option)
        {
            return Ok(await _itemUserService.GetUserAvailableItemsAsync(option));
        }

        [HttpGet]
        public async Task<IActionResult> GetItem(Guid id)
        {
            return Ok(await _itemUserService.GetItemAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> CheckAvailableItem(Guid id)
        {
            await _itemUserService.CheckAvailableItemAsync(id);
            return Ok();
        }
    }
}
