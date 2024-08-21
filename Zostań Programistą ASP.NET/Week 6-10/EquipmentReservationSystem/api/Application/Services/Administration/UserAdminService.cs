using Application.Attributes;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Application.Interfaces.IUserAdminService;

namespace Application.Services
{
    [AutoRegisterTransientService(typeof(IUserAdminService))]
    public partial class UserAdminService : IUserAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly UserManager<UserData> _userManager;
        private readonly ILogService _logger;

        public UserAdminService(IUserRepository userRepository, UserManager<UserData> userManager, IRoleRepository roleRepository, ILogService logService)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _logger = logService;
        }

        public UserPanelAccessModel GetPanelAccess()
        {
            var isAdmin = _roleRepository.CheckUserHasRoleAsync(Constans.Constans.Role.Name.Administration).Result;

            if (isAdmin) return new IUserAdminService.UserPanelAccessModel()
            {
                Admin = true,
                PickUpPoint = true
            };

            return new UserPanelAccessModel()
            {
                Admin = false,
                PickUpPoint = _roleRepository.CheckUserHasRoleAsync(Constans.Constans.Role.Name.PickupPoint).Result
            };
        }

        public async Task<ICollection<UserTableModel>> GetAllAsync()
        {
            var list = await _userRepository.QueryBuilder(asNoTracking: true)
                .IncludeDepartments()
                .IncludeReservation()
                .Query
                .OrderBy(o => o.UserName)
                .Select(s => new UserTableModel()
                {
                    Id = s.Id,
                    Active = s.Active,
                    Username = s.UserName,
                    Email = s.Email,
                    Phone = s.PhoneNumber,
                    DepartmentsCount = s.Departments.Count(x => x.Department.Active),
                    RentedItemsCount = s.Reservations.Count(x => x.Status == Domain.Utils.ReservationStatus.Issued || x.Status == Domain.Utils.ReservationStatus.ReadyToPickedUp),
                }).ToListAsync();

            foreach (var user in list)
            {
                var rolse = _userRepository.GetUserRoles(user.Id);

                user.IsAdmin = rolse.Any(x => x.Id == Constans.Constans.Role.Id.Administration);
                user.IsPickupPoint = rolse.Any(x => x.Id == Constans.Constans.Role.Id.PickupPoint);
            }

            return list;
        }

        public async Task<ICollection<UserComboModel>> GetAllComboAsync(UserComboParams filter)
        {
            var query = _userRepository.QueryBuilder(true, true)
                .Query
                .OrderBy(o => o.UserName)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter?.Search)) query = query.Where(x => x.UserName.Contains(filter.Search));
            if (filter?.Skip != null) query = query.Skip(filter.Skip.Value);
            if (filter?.Take != null) query = query.Take(filter.Take.Value);

            var users = await query.Select(s => new UserComboModel()
            {
                Id = s.Id,
                Username = s.UserName,
                Email = s.Email,
            }).ToListAsync();

            return users;
        }

        public async Task<UserModel> GetFullAsync(Guid id)
        {
            var entity = await _userRepository.QueryBuilder(asNoTracking: true)
                .IncludeDepartments()
                .IncludeReservationItem()
                .GetUserByIdAsync(id) ?? throw new UserNotFoundException();

            var model = new UserModel()
            {
                Id = entity.Id,
                Active = entity.Active,
                Username = entity.UserName,
                Email = entity.Email,
                Phone = entity.PhoneNumber,
                Created = DateOnly.FromDateTime(entity.CreateTime),

                isAdmin = await _roleRepository.CheckUserHasRoleAsync(Constans.Constans.Role.Name.Administration, entity.Id),
                isPickupPoint = await _roleRepository.CheckUserHasRoleAsync(Constans.Constans.Role.Name.PickupPoint, entity.Id),

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

        public async Task<Guid> Create(UserModel model)
        {
            try
            {
                await _userRepository.Transactions.BeginTransactionAsync();

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

                await _userRepository.Transactions.CommitTransactionAsync();
                await _logger.InfoLogAsync($"Successfully create user ({newUser.UserName}, {newUser.Id})", source: typeof(UserAdminService).Name);
                return newUser.Id;
            }
            catch (Exception)
            {
                await _userRepository.Transactions.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task Update(UserModel model)
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
            if (beforeRoles.Any(x => x.Id == Constans.Constans.Role.Id.Administration) && !model.isAdmin) await _roleRepository.RemoveRoleFromUserAsync(Constans.Constans.Role.Id.Administration, entity.Id);

            if (!beforeRoles.Any(x => x.Id == Constans.Constans.Role.Id.PickupPoint) && model.isPickupPoint) await _roleRepository.AddRoleToUserAsync(Constans.Constans.Role.Id.PickupPoint, entity.Id);
            if (beforeRoles.Any(x => x.Id == Constans.Constans.Role.Id.PickupPoint) && !model.isPickupPoint) await _roleRepository.RemoveRoleFromUserAsync(Constans.Constans.Role.Id.PickupPoint, entity.Id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = _userRepository.GetUser(id);
            await _logger.InfoLogAsync($"Delete user ({user.UserName}, {user.Id})", source: typeof(UserAdminService).Name);
            await _userRepository.DeleteAsync(id);
        }

        public IPaginationTable<UserTableModel>.TableData GetTable(IPaginationTable<UserTableModel>.TableOptions options)
        {
            var data = new IPaginationTable<UserTableModel>.TableData();

            var query = _userRepository.QueryBuilder(false, true, false)
                .IncludeDepartments()
                .IncludeReservation()
                .Query;

            if (!string.IsNullOrWhiteSpace(options.Search)) query = query.Where(x => x.UserName.Contains(options.Search));

            data.TotalCount = query.Count();
            query = query.OrderBy(o => o.UserName);
            query = query.Skip(options.Skip ?? 0);
            query = query.Take(options.Take ?? 0);

            data.Data = query.Select(s => new UserTableModel
            {
                Id = s.Id,
                Active = s.Active,
                Username = s.UserName,
                Email = s.Email,
                Phone = s.PhoneNumber,
                DepartmentsCount = s.Departments.Count(x => x.Department.Active),
                RentedItemsCount = s.Reservations.Count(x => x.Status == Domain.Utils.ReservationStatus.InPreparation),
            }).ToList();

            foreach (var item in data.Data)
            {
                item.IsAdmin = _roleRepository.CheckUserHasRoleAsync(Constans.Constans.Role.Id.Administration, item.Id).Result;
                item.IsPickupPoint = _roleRepository.CheckUserHasRoleAsync(Constans.Constans.Role.Id.PickupPoint, item.Id).Result;
            }

            return data;
        }

        public async Task<IPaginationTable<UserTableModel>.TableData> GetTablAsync(IPaginationTable<UserTableModel>.TableOptions options)
        {
            var data = new IPaginationTable<UserTableModel>.TableData();

            var query = _userRepository.QueryBuilder(false, true, false)
                .IncludeDepartments()
                .IncludeReservation()
                .Query;

            if (!string.IsNullOrWhiteSpace(options.Search)) query = query.Where(x => x.UserName.Contains(options.Search));

            data.TotalCount = await query.CountAsync();
            query = query.OrderBy(o => o.UserName);
            query = query.Skip(options.Skip ?? 0);
            query = query.Take(options.Take ?? 0);

            data.Data = await query.Select(s => new UserTableModel
            {
                Id = s.Id,
                Active = s.Active,
                Username = s.UserName,
                Email = s.Email,
                Phone = s.PhoneNumber,
                DepartmentsCount = s.Departments.Count(x => x.Department.Active),
                RentedItemsCount = s.Reservations.Count(x => x.Status == Domain.Utils.ReservationStatus.InPreparation),
            }).ToListAsync();

            foreach (var item in data.Data)
            {
                item.IsAdmin = await _roleRepository.CheckUserHasRoleAsync(Constans.Constans.Role.Id.Administration, item.Id);
                item.IsPickupPoint = await _roleRepository.CheckUserHasRoleAsync(Constans.Constans.Role.Id.PickupPoint, item.Id);
            }

            return data;
        }

        public async Task<bool> CheckUserEmailUnique(CheckUniqueModel data)
        {
            return !await _userRepository.QueryBuilder(asNoTracking: true, allowBuffor: true)
                .Where(x => x.Id != data.Id && x.NormalizedEmail == data.Value.ToUpper())
                .Query
                .AnyAsync();
        }

        public async Task<bool> CheckUsernameUnique(CheckUniqueModel data)
        {
            return !await _userRepository.QueryBuilder(asNoTracking: true, allowBuffor: true)
                .Where(x => x.Id != data.Id && x.NormalizedUserName == data.Value.ToUpper())
                .Query
                .AnyAsync();
        }
    }
}
