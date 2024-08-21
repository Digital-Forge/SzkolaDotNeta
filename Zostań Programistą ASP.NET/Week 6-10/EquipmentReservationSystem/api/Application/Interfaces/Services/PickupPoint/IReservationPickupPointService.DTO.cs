using Domain.Utils;

namespace Application.Interfaces.Services.PickupPoint
{
    public partial interface IReservationPickupPointService
    {
        class ReservationTableModel
        {
            public Guid Id { get; set; }
            public string ItemName { get; set; }
            public string Username { get; set; }
            public DateOnly From { get; set; }
            public DateOnly? To { get; set; }
            public ReservationStatus Status { get; set; }
            public Guid Image { get; set; }
        }

        class PreparationAndReleaseReservationsTableOption : IPaginationTable<ReservationTableModel>.TableOptions
        {
            public List<Guid>? SearchItems { get; set; } = new();
            public List<Guid>? SearchUsers { get; set; } = new();
            public ReservationShowType ShowType { get; set; }

            public enum ReservationShowType 
            {
                All,
                InPreparation,
                ReadyToPickedUp
            }
        }

        class ReturnsReservationsTableOption : IPaginationTable<ReservationTableModel>.TableOptions
        {
            public List<Guid>? SearchItems { get; set; } = new();
            public List<Guid>? SearchUsers { get; set; } = new();
        }

        class ReservationModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string SerialNumber { get; set; }
            public string Username { get; set; }
            public DateOnly From { get; set; }
            public DateOnly? To { get; set; }
            public ReservationStatus Status { get; set; }
            public string InnerNote { get; set; }
            public List<Guid> Images { get; set; } = new();
            public List<FileInfoModel> Files { get; set; } = new();


            public class FileInfoModel
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public bool IsMissing { get; set; }
            }
        }

        class ChangeReservationModel
        {
            public Guid ReservationId { get; set; }
            public ReservationStatus Status { get; set; }
            public bool AdminMode { get; set; }
            public DateOnly? From { get; set; }
            public DateOnly? To { get; set; }
            public string InnerNote { get; set; }
        }
    }
}
