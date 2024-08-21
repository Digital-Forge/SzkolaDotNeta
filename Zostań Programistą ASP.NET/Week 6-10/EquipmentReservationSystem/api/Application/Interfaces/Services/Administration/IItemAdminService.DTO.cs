using Domain.Utils;

namespace Application.Interfaces
{
    public partial interface IItemAdminService
    {
        class ItemTableModel
        {
            public Guid Id { get; set; }
            public bool Active { get; set; }
            public string Name { get; set; }
            public int Count { get; set; }
            public string ImageId { get; set; }
        }

        class ItemTableOprtion : TableOptions
        {
            public List<Guid> AvailableInDepartments { get; set; } = new List<Guid>();
        }

        class ItemComboModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        class ItemComboModelWithImage
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string ImageId { get; set; }
        }

        class ItemModel
        {
            public Guid? Id { get; set; }
            public bool Active { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public DateOnly? Create { get; set; }
            public List<DepartmentModel> Departments { get; set; } = new List<DepartmentModel>();
            public List<Instance> Instances { get; set; } = new List<Instance>();
            public List<Guid> ImagesId { get; set; } = new List<Guid>();
            public List<AddictionFile> AddictionFiles { get; set; } = new List<AddictionFile>();
            public List<ReservationHistory> ReservationsHistory { get; set; } = new List<ReservationHistory>();


            public class Instance
            {
                public Guid? Id { get; set; }
                public bool Active { get; set; }
                public string SerialNumber { get; set; }
                public ItemInstanceStatus Status { get; set; }
                public DateOnly AddedDate { get; set; }
                public DateOnly? WithdrawalDate { get; set; }
            }

            public class ReservationHistory
            {
                public Guid Id { get; set; }
                public DateOnly From { get; set; }
                public DateOnly? To { get; set; }
                public string SerialNumber { get; set; }
                public string Who { get; set; }
                public ReservationStatus Status { get; set; }
            }

            public class AddictionFile
            {
                public Guid? Id { get; set; }
                public bool Active { get; set; }
                public string Name { get; set; }
                public DateTime Date { get; set; }
            }

            public class DepartmentModel
            {
                public Guid? Id { get; set; }
                public string Name { get; set; }
            }
        }
    }
}
