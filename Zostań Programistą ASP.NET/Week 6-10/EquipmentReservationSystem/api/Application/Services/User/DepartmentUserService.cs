using Application.Attributes;
using Application.Interfaces;
using Application.Interfaces.Services.User;
using Domain.Interfaces.Repositories;

namespace Application.Services.User
{
    [AutoRegisterTransientService(typeof(IDepartmentUserService))]
    public class DepartmentUserService(IUserRepository _userRepository) : IDepartmentUserService
    {
        public async Task<IComboBoxApi<Guid>.ResponsData> GetUserAvailableDepartmentsAsync(IComboBoxApi<Guid>.SearchOption search, Guid? userId = null)
        {
            userId ??= _userRepository.GetContextUser().Id;

            var toRespons = new IComboBoxApi<Guid>.ResponsData();

            var userDepartments = _userRepository.GetUserDepartments(userId.Value);

            toRespons.Data = userDepartments
               .Where(x => x.Name.ToLower().Contains(search.Search.ToLower()))
               .OrderBy(x => x.Name)
               .Take(search.Take ?? int.MaxValue)
               .Select(s => new IComboBoxApi<Guid>.ResponsData.PositionData()
               {
                   Code = s.Id,
                   Name = s.Name,
               }).ToList();

            if ((search.SerchingPosition?.Count ?? 0) != 0)
                toRespons.SerchingPosition = userDepartments
                    .Where(x => search.SerchingPosition?.Contains(x.Id) ?? false)
                    .Select(s => new IComboBoxApi<Guid>.ResponsData.PositionData()
                    {
                        Code = s.Id,
                        Name = s.Name,
                    }).ToList();

            return toRespons;
        }
    }
}
