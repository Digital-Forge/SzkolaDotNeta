using Domain.Utils;

namespace Application.Interfaces.Services.PickupPoint
{
    public partial interface IReservationPickupPointService
    {
        Task<IPaginationTable<ReservationTableModel>.TableData> GetPreparationAndReleaseReservationsAsync(PreparationAndReleaseReservationsTableOption options);
        Task<IPaginationTable<ReservationTableModel>.TableData> GetItemsToReturnAsync(ReturnsReservationsTableOption options);
        Task ChangeReservationStatusAsync(ChangeReservationModel model);
        Task<ReservationModel> GetReservationInfoAsync(Guid id);
        Task<IComboBoxApi<Guid>.ResponsData> GetReservedItemWithStatusAsync(IComboBoxApi<Guid>.SearchOption search, params ReservationStatus[] status);
        Task<IComboBoxApi<Guid>.ResponsData> GetReservedUserWithStatusAsync(IComboBoxApi<Guid>.SearchOption search, params ReservationStatus[] status);
    }
}
