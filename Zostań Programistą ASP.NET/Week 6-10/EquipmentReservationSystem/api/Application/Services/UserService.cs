using Application.Attributes;
using Application.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Application.Interfaces.IUserService;

namespace Application.Services
{
    [AutoRegisterTransientService(typeof(IUserService))]
    public partial class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly UserManager<UserData> _userManager;

        public UserService(IUserRepository userRepository, UserManager<UserData> userManager, IRoleRepository roleRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public UserPanelAccessModel GetPanelAccess()
        {
            var isAdmin = _roleRepository.CheckUserHasRole(Constans.Constans.Role.Name.Administration).Result;

            if (isAdmin) return new IUserService.UserPanelAccessModel()
            {
                Admin = true,
                PickUpPoint = true
            };

            return new UserPanelAccessModel()
            {
                Admin = false,
                PickUpPoint = _roleRepository.CheckUserHasRole(Constans.Constans.Role.Name.PickupPoint).Result
            };
        }

        public async Task<ICollection<UserBaseModel>> GetAllAsync()
        {
            var list = await _userRepository.QueryBuilder(asNoTracking: true)
                .IncludeDepartments()
                .IncludeReservation()
                .Query.Select(s => new UserBaseModel()
                {
                    Id = s.Id,
                    Active = s.Active,
                    Username = s.UserName,
                    Email = s.Email,
                    Phone = s.PhoneNumber,
                    DepartmentsCount = s.Departments.Count(),
                    RentedItemsCount = s.Reservations.Count(x => x.Status == Domain.Utils.ReservationStatus.InPreparation),
                }).ToListAsync();

            foreach (var user in list)
            {
                var rolse = _userRepository.GetUserRoles(user.Id);

                user.IsAdmin = rolse.Any(x => x.Id == Constans.Constans.Role.Id.Administration);
                user.IsPickupPoint = rolse.Any(x => x.Id == Constans.Constans.Role.Id.PickupPoint);
            }

            return list;
        }

        public async Task<UserFullModel> GetFullAsync(Guid id)
        {
            var entity = await _userRepository.QueryBuilder(asNoTracking: true)
                .IncludeReservation()
                .IncludeDepartments()
                .GetUserByIdAsync(id) ?? throw new UserNotFoundException();

            var model = new UserFullModel()
            {
                Id = entity.Id,
                Active = entity.Active,
                Username = entity.UserName,
                Email = entity.Email,
                Phone = entity.PhoneNumber,
                Created = DateOnly.FromDateTime(entity.CreateTime),

                isAdmin = await _roleRepository.CheckUserHasRole(Constans.Constans.Role.Name.Administration, entity.Id),
                isPickupPoint = await _roleRepository.CheckUserHasRole(Constans.Constans.Role.Name.PickupPoint, entity.Id),

                Departments = entity.Departments.Select(s => new UserDepartmentsModel()
                {
                    Id = s.Department.Id,
                    Name = s.Department.Name,
                })
                .OrderBy(o => o.Name)
                .ToList(),

                ItemHistory = entity.Reservations.Select(s => new UserItemsModel()
                {
                    Id = s.Id,
                    From = s.From,
                    To = s.To,
                    Name = s.ItemInstance.Item.Name,
                    Status = s.Status.ToString(),
                })
                .OrderBy(o => o.From)
                .ThenBy(o => o.To)
                .ToList()
            };

            return model;
        }

        public async Task<Guid> Create(UserFullModel model)
        {
            var newUser = new UserData()
            {
                Email = model.Email,
                UserName = model.Username,
                NormalizedUserName = model.Username.ToUpper(),
                Active = model.Active,
                EntityStatus = Domain.Utils.EntityStatus.Buffer,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded) throw new UserCreateException();

            if (model.isAdmin) await _roleRepository.AddRoleToUserAsync(Constans.Constans.Role.Id.Administration, newUser.Id);
            if (model.isPickupPoint) await _roleRepository.AddRoleToUserAsync(Constans.Constans.Role.Id.PickupPoint, newUser.Id);

            newUser.Departments = model.Departments.Select(s => new Domain.Models.Business.MiddleTabs.UserToDepartment
            {
                DepartmentId = s.Id,
                UserId = newUser.Id,
            }).ToList();

            newUser.EntityStatus = Domain.Utils.EntityStatus.Use;
            await _userRepository.SaveAsync(newUser);
            return newUser.Id;
        }

        public async Task Update(UserFullModel model)
        {
            var entity = await _userRepository.QueryBuilder()
                .IncludeDepartments()
                .IncludeReservation()
                .GetUserByIdAsync(model.Id.Value) ?? throw new UserNotFoundException();

            entity.UserName = model.Username;
            entity.NormalizedUserName = model.Username.ToUpper();
            entity.Email = model.Email;
            entity.NormalizedEmail = model.Email.ToUpper();
            entity.PhoneNumber = model.Phone;
            entity.Active = model.Active;

            if (!string.IsNullOrWhiteSpace(model.Password)) entity.PasswordHash = _userManager.PasswordHasher.HashPassword(entity, model.Password);

            entity.Departments = model.Departments.Select(s => new Domain.Models.Business.MiddleTabs.UserToDepartment
            {
                DepartmentId = s.Id,
                UserId = entity.Id,
            }).ToList();

            await _userRepository.SaveAsync(entity);

            var beforeRoles = _userRepository.GetUserRoles(entity.Id);

            if (!beforeRoles.Any(x => x.Id == Constans.Constans.Role.Id.Administration) && model.isAdmin) await _roleRepository.AddRoleToUserAsync(Constans.Constans.Role.Id.Administration, entity.Id);
            if (beforeRoles.Any(x => x.Id == Constans.Constans.Role.Id.Administration) && !model.isAdmin) await _roleRepository.RemoveRoleFromUser(Constans.Constans.Role.Id.Administration, entity.Id);

            if (!beforeRoles.Any(x => x.Id == Constans.Constans.Role.Id.PickupPoint) && model.isPickupPoint) await _roleRepository.AddRoleToUserAsync(Constans.Constans.Role.Id.PickupPoint, entity.Id);
            if (beforeRoles.Any(x => x.Id == Constans.Constans.Role.Id.PickupPoint) && !model.isPickupPoint) await _roleRepository.RemoveRoleFromUser(Constans.Constans.Role.Id.PickupPoint, entity.Id);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}
