using Domain.Utils;

namespace Application.Interfaces.Services.User
{
    public partial interface IReservationUserService
    {
        class ReservationTableModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public Guid ImageId { get; set; }
            public DateOnly From { get; set; }
            public DateOnly? To { get; set; }
            public ReservationStatus Status { get; set; }
        }

        class ReservationTableOptions : IPaginationTable<string>.TableOptions
        {
            public List<Guid>? AvailableInDepartments { get; set; } = new List<Guid>();
        }

        class ReservationModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string SerialNumber { get; set; }
            public DateOnly From { get; set; }
            public DateOnly? To { get; set; }
            public ReservationStatus Status { get; set; }
            public List<Guid> Images { get; set; } = new();
            public List<ItemFile> Files { get; set; } = new();


            public class ItemFile
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
            }
        }

        class CreateReservationModel
        {
            public Guid ItemId { get; set; }
            public DateOnly From { get; set; }
            public DateOnly To { get; set; }
        }
    }
}
