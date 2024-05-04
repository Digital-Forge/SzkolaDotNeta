namespace Application.Interfaces
{
    public partial interface IDepartmentAdminService
    {
        public class DepartmentTableModel
        {
            public Guid? Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public bool Active { get; set; }
            public int UserCount { get; set; }
            public int ItemTypeCount { get; set; }
        }

        public class DepartmentComboModel
        {
            public Guid? Id { get; set; }
            public string Name { get; set; }
        }

        public class DepartmentModel
        {
            public Guid? Id { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public bool Active { get; set; }
            public DateOnly? Create { get; set; }
            public List<DepartmentUserModel> Users { get; set; }
            public List<DepartmentItemModel> Items { get; set; }
        }

        public class DepartmentUserModel {
            public Guid Id { get; set;}
            public string Fullname { get; set; }
            public string Email { get; set; }

        }
        public class DepartmentItemModel {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
