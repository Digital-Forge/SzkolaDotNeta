using Application.Interfaces.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Interfaces.Services.User.IReservationUserService;

namespace Web.Controllers.User
{
    [Authorize]
    [ApiController]
    [Route("api/Reservation/[action]")]
    public class ReservationUserController(IReservationUserService _reservationUserService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> MyReservation(ReservationTableOptions option)
        {
            return Ok(await _reservationUserService.GetUserReservationAsync(option));
        }

        [HttpPost]
        public async Task<IActionResult> MyReservationHistory(ReservationTableOptions option)
        {
            return Ok(await _reservationUserService.GetUserReservationHistoryAsync(option));
        }

        [HttpGet]
        public async Task<IActionResult> GetReservation(Guid id)
        {
            return Ok(await _reservationUserService.GetReservationAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(CreateReservationModel model)
        {
            await _reservationUserService.CreateReservationAsync(model);
            return Ok();
        }
    }
}
