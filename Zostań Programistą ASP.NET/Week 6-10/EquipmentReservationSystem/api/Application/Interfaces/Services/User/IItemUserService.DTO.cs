namespace Application.Interfaces.Services.User
{
    public partial interface IItemUserService
    {
        class ItemModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public List<Guid> Images { get; set; } = new List<Guid>();
            public List<FileInfoModel> Files { get; set; } = new List<FileInfoModel>();

            public class FileInfoModel
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
            }
        }

        class ItemTableModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public Guid ImageId { get; set; }
            public int AvailableQuantity { get; set; }
        }

        class ItemTableOption : IPaginationTable<ItemTableModel>.TableOptions
        {
            public List<Guid>? AvailableInDepartments { get; set; } = new List<Guid>();
        }
    }
}
