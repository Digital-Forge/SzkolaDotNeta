using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ResourceManagementSystem.Application.Interfaces;
using ResourceManagementSystem.Application.ViewModel.Users;
using ResourceManagementSystem.Domain.Interface;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ResourceManagementSystem.Application.Services
{
    public class UsersModerateService : IUsersModerateService
    {
        private readonly IUserRepository _userRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IDepartmentRepository _departmentRepo;
        private readonly IReservationRepository _reservationRepo;
        private readonly IAccessConfigRepository _accessConfigRepo;
        private readonly IMapper _mapper;

        public UsersModerateService(IUserRepository userRepository,
                                    UserManager<AppUser> userManager,
                                    IDepartmentRepository departmentRepository,
                                    IReservationRepository reservationRepository,
                                    IAccessConfigRepository accessConfigRepository,
                                    IMapper mapper)
        {
            _userManager = userManager;
            _userRepo = userRepository;
            _departmentRepo = departmentRepository;
            _reservationRepo = reservationRepository;
            _accessConfigRepo = accessConfigRepository;
            _mapper = mapper;
        }

        public List<UserOfListVM> ListOfUsers()
        {
            return _userRepo.GetUsersList().Select(x => new UserOfListVM
            {
                Index = x.Id,
                FullName = x.FullName,
                Email = string.IsNullOrEmpty(x.Email) ? "" : x.Email,
                Phone = string.IsNullOrEmpty(x.PhoneNumber) ? "" : x.PhoneNumber,
                isActive = x.isActive,
                CountOfDepartments = x.Departments.Count(),
                CountOfResources = x.Reservations.Count()
            }).ToList();
        }

        public CreateUserVM CreateUser(ClaimsPrincipal user)
        {
            var buff = new CreateUserVM();

            if (user.IsInRole("Admin"))
            {
                buff.DepartmentsList = _departmentRepo.GetDepartmentsList()
                    .Select(x => new AddRemoveStatusVM
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Status = false
                    }).ToList();

                buff.RolesList = _accessConfigRepo.GetRolesList()
                    .Select(x => new AddRemoveStatusVM
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Status = false
                    }).ToList();
            }
            return buff;
        }

        public string CreateUser(CreateUserVM input, ClaimsPrincipal user)
        {
            var newUser = new AppUser
            {
                UserName = input.Username,
                FullName = input.FullName,
                PhoneNumber = input.Phone
            };

            var result = _userManager.CreateAsync(newUser, input.Password).Result;

            if (!result.Succeeded)
            {
                if (_userRepo.GetUsersList().Where(x => x.UserName == input.Username).Count() != 0) return "1";
                else return "-1";
            }
            else
            {
                newUser = _userManager.Users.Where(x => x.UserName == newUser.UserName).FirstOrDefault();

                _userManager.SetEmailAsync(newUser, input.Email);
                SetActiveUser(input.SetActive, newUser);

                foreach (var role in input.RolesList)
                {
                    if (role.Status)
                    {
                        _userManager.AddToRoleAsync(newUser, role.Name);
                    }
                }

                foreach (var department in input.DepartmentsList)
                {
                    if (department.Status)
                    {
                        _departmentRepo.AddUserToDepartment(newUser.Id, department.Id);
                    }
                }
            }
            return newUser.Id;
        }

        public bool SetActiveUser(bool status, AppUser user)
        {
            if (status)
            {
                if (!_userManager.IsInRoleAsync(user, "User").Result)
                {
                    if (!_userManager.AddToRoleAsync(user, "User").Result.Succeeded) return false;
                }
            }
            else
            {
                if (_userManager.IsInRoleAsync(user, "User").Result)
                {
                    if (!_userManager.RemoveFromRoleAsync(user, "User").Result.Succeeded) return false;
                }
            }

            if (user.isActive != status)
            {
                user.isActive = status;
                return _userRepo.UpdateUser(user);
            }
            return true;
        }

        public DetailsEditUserVM UserDetails(string userId)
        {
            var user = _userRepo.GetUserById(userId);
            if (user == null) return null;

            var VM = _mapper.Map<DetailsEditUserVM>(user);
            if (VM == null) return null;

            foreach (var department in user.Departments)
            {
                VM.DepartmentsList.Add(new AddRemoveStatusVM { Name = department.Department.Name });
            }

            foreach (var role in _accessConfigRepo.GetRolesList())
            {
                if (_userManager.IsInRoleAsync(user, role.Name).Result && role.Name != "User")
                {
                    VM.RolesList.Add(new AddRemoveStatusVM { Name = role.Name });
                } 
            }

            foreach (var item in _reservationRepo.GetReservationListByUser(userId))
            {
                VM.ReservationItemList.Add(new AddRemoveStatusVM { Name = item.Item.Name });
            }

            return VM;
        }

        public DetailsEditUserVM GetEditUser(string userId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateEditUser(DetailsEditUserVM input)
        {
            throw new NotImplementedException();
        }
    }
}
