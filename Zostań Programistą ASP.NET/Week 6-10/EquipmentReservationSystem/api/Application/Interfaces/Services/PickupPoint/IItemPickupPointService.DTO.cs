using Domain.Utils;

namespace Application.Interfaces.Services.PickupPoint
{
    public partial interface IItemPickupPointService
    {
        class ServiceTableModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }

            // ... uzupełnić

            public ItemInstanceStatus Status { get; set; }
        }

        class ServiceTableOption : IPaginationTable<ServiceTableModel>.TableOptions
        {
            public List<Guid>? SearchItems { get; set; } = new List<Guid>();
        }
    }
}
