using Application.Constans;
using Application.Interfaces;
using Application.Interfaces.Services.PickupPoint;
using Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Interfaces.Services.PickupPoint.IReservationPickupPointService;

namespace Web.Controllers.PickupPoint
{
    [ApiController]
    [Route("api/PickupPoint/Reservation/[action]")]
    [Authorize(Roles = $"{Constans.Role.Name.PickupPoint},{Constans.Role.Name.Administration}")]
    public class ReservationPickupPointController(IReservationPickupPointService _reservationService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> GetPreparationAndReleaseReservations(PreparationAndReleaseReservationsTableOption options)
        {            
            return Ok(await _reservationService.GetPreparationAndReleaseReservationsAsync(options));
        }

        [HttpPost]
        public async Task<IActionResult> GetItemsToReturn(ReturnsReservationsTableOption options)
        {
            return Ok(await _reservationService.GetItemsToReturnAsync(options));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeReservationStatus(ChangeReservationModel model)
        {
            await _reservationService.ChangeReservationStatusAsync(model);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetReservationInfo(Guid id)
        {
            return Ok(await _reservationService.GetReservationInfoAsync(id));
        }

        // PreparationAndReleaseReservations
        [HttpPost]
        public async Task<IActionResult> GetAvailableReservedItemToPreparationAndRelease(IComboBoxApi<Guid>.SearchOption option)
        {
            return Ok(await _reservationService.GetReservedItemWithStatusAsync(option, ReservationStatus.InPreparation, ReservationStatus.ReadyToPickedUp));
        }

        [HttpPost]
        public async Task<IActionResult> GetAvailableReservedUserToPreparationAndRelease(IComboBoxApi<Guid>.SearchOption option)
        {
            return Ok(await _reservationService.GetReservedUserWithStatusAsync(option, ReservationStatus.InPreparation, ReservationStatus.ReadyToPickedUp));
        }

        // Returns
        [HttpPost]
        public async Task<IActionResult> GetAvailableReservedItemToReturns(IComboBoxApi<Guid>.SearchOption option)
        {
            return Ok(await _reservationService.GetReservedItemWithStatusAsync(option, ReservationStatus.Issued));
        }

        [HttpPost]
        public async Task<IActionResult> GetAvailableReservedUserToReturns(IComboBoxApi<Guid>.SearchOption option)
        {
            return Ok(await _reservationService.GetReservedUserWithStatusAsync(option, ReservationStatus.Issued));
        }
    }
}
