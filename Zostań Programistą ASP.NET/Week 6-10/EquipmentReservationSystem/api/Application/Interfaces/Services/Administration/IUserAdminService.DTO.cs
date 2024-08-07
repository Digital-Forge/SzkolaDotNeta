namespace Application.Interfaces
{
    public partial interface IUserAdminService
    {
        class UserPanelAccessModel
        {
            public bool PickUpPoint { get; set; }
            public bool Admin { get; set; }
        }

        class UserComboModel
        {
            public Guid Id { get; set; }
            public string Email { get; set; }
            public string Username { get; set; }
        }

        class UserComboParams
        {
            public string? Search { get; set; }
            public int? Take { get; set; } = null;
            public int? Skip { get; set; } = null;
        }

        class UserTableModel
        {
            public Guid Id { get; set; }
            public bool Active { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Username { get; set; }
            public bool IsAdmin { get; set; }
            public bool IsPickupPoint { get; set; }
            public int RentedItemsCount { get; set; }
            public int DepartmentsCount { get; set; }
        }

        class UserModel
        {
            public Guid? Id { get; set; }
            public bool Active { get; set; }
            public string Email { get; set; }
            public string? Password { get; set; }
            public string? Phone { get; set; }
            public string Username { get; set; }
            public bool isAdmin { get; set; }
            public bool isPickupPoint { get; set; }
            public List<UserItemsModel> ItemHistory { get; set; }
            public List<UserDepartmentsModel> Departments { get; set; }
            public DateOnly? Created { get; set; }
        }

        class UserItemsModel
        {
            public Guid Id { get; set; }
            public DateOnly From { get; set; }
            public DateOnly? To { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
        }

        class UserDepartmentsModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        class CheckUniqueModel
        {
            public Guid? Id { get; set; }
            public string Value { get; set; }
        }
    }
}
