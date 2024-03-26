
namespace Application.Interfaces
{
    public partial interface IUserService
    {
        UserPanelAccessModel GetPanelAccess();

        Task<ICollection<UserTableModel>> GetAllAsync();
        Task<ICollection<UserComboModel>> GetAllComboAsync(UserComboParams filter);
        Task DeleteAsync(Guid id);
        Task<UserModel> GetFullAsync(Guid id);
        Task<Guid> Create(UserModel model);
        Task Update(UserModel model);
    }
}
