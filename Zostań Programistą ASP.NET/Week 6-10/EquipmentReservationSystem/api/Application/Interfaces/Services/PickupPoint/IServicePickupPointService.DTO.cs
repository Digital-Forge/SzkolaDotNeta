namespace Application.Interfaces.Services.PickupPoint
{
    public partial interface IServicePickupPointService
    {
        class ServiceItemTableModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string SerialNumber { get; set; }
            public Guid? ImageId { get; set; }     
        }

        class ServiceItemTableOption : IPaginationTable<ServiceItemTableModel>.TableOptions
        {
            public List<Guid>? SearchItems { get; set; } = new();
        }

        class ServiceItemModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string SerialNumber { get; set; }
            public string Description { get; set; }
            public LastReservation LastReservationInfo { get; set; }
            public string ServiceNote { get; set; }
            public List<string> ServiceNoteList { get; set; }
            public List<Guid> Images { get; set; }
            public List<IFileService.DataFileInfoModel> Files { get; set; }

            public class LastReservation
            {
                public DateOnly From { get; set; }
                public DateOnly? To { get; set; }
                public string Username { get; set; }
            }
        }

        class ChangeStatusServiceItemModel
        {
            public Guid Id { get; set; }
            public ServiceStatus Status { get; set; }
            public string? ServiceNote { get; set; }


            public enum ServiceStatus
            {
                Serviced, 
                Repaired, 
                Destroyed
            }
        }
    }
}
