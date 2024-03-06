namespace Application.Interfaces
{
    public partial interface IDepartmentService
    {
        public class DepartmentBaseModel
        {
            public Guid? Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int UserCount { get; set; }
            public int ItemTypeCount { get; set; }
        }

        public class DepartmentFullModel
        {
            public Guid? Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateOnly? Create { get; set; }
            public List<DepartmentUserModel> Users { get; set; }
            public List<DepartmentItemModel> Items { get; set; }
        }

        public class DepartmentUserModel {
            public Guid Id { get; set;}
            public string Fullname { get; set; }

        }
        public class DepartmentItemModel {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
