namespace Application.Interfaces.Services.User
{
    public partial interface IDepartmentUserService
    {
        Task<IComboBoxApi<Guid>.ResponsData> GetUserAvailableDepartmentsAsync(IComboBoxApi<Guid>.SearchOption search, Guid? userId = null);
    }
}
