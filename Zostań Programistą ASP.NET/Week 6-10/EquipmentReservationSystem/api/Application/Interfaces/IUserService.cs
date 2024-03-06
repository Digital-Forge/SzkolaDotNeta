
namespace Application.Interfaces
{
    public partial interface IUserService
    {
        UserPanelAccessModel GetPanelAccess();

        Task<ICollection<UserBaseModel>> GetAllAsync();
        Task DeleteAsync(Guid id);
        Task<UserFullModel> GetFullAsync(Guid id);
        Task<Guid> Create(UserFullModel model);
        Task Update(UserFullModel model);
    }
}
