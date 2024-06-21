namespace Application.Interfaces.Services.User
{
    public partial interface IReservationUserService
    {
        Task<IPaginationTable<ReservationTableModel>.TableData> GetUserReservationAsync(ReservationTableOptions option, Guid? Id = null);
        Task<IPaginationTable<ReservationTableModel>.TableData> GetUserReservationHistoryAsync(ReservationTableOptions option, Guid? Id = null);
        Task<ReservationModel> GetReservationAsync(Guid id);
        Task CreateReservationAsync(CreateReservationModel model, Guid? userId = null);
    }
}
