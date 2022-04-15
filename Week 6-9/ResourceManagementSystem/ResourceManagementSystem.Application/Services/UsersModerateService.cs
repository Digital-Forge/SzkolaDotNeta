using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ResourceManagementSystem.Application.Interfaces;
using ResourceManagementSystem.Application.ViewModel.Users;
using ResourceManagementSystem.Application.ViewModel.ExtraViewModel;
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
                        Id = x.Id.ToString().ToLower(),
                        Name = x.Name,
                        Status = false
                    }).ToList();

                buff.RolesList = _accessConfigRepo.GetRolesList()
                    .Where(x => x.Name != "User")
                    .Select(x => new AddRemoveStatusVM
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Status = false
                    }).ToList();
            }
            return buff;
        }

        public string CreateUser(CreateUserVM input, ClaimsPrincipal moderator)
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

                _userManager.SetEmailAsync(newUser, input.Email).Wait();
                SetActiveUser(input.SetActive, newUser);

                if(moderator.IsInRole("Admin"))
                {
                    foreach (var role in input.RolesList.ToList())
                    {
                        if (role.Status)
                        {
                            _accessConfigRepo.AddRoleToUser(role.Name, newUser);
                        }
                    }

                    foreach (var department in input.DepartmentsList.ToList())
                    {
                        if (department.Status)
                        {
                            _departmentRepo.AddUserToDepartment(newUser.Id, department.Id);
                        }
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
                    if (!_accessConfigRepo.AddRoleToUser("User", user)) return false;
                }
            }
            else
            {
                if (_userManager.IsInRoleAsync(user, "User").Result)
                {
                    if (!_accessConfigRepo.RemoveRoleFromUser("User", user)) return false;
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

            foreach (var department in _departmentRepo.GetDepartmentsListByUser(userId))
            {
                VM.DepartmentsList.Add(new AddRemoveStatusVM { Name = department.Name });
            }

            foreach (var role in _accessConfigRepo.GetRolesList().ToList())
            {
                if (_userManager.IsInRoleAsync(user, role.Name).Result && role.Name != "User")
                {
                    VM.RolesList.Add(new AddRemoveStatusVM { Name = role.Name });
                } 
            }

            foreach (var item in _reservationRepo.GetReservationListByUser(userId).Select(x => x.Item))
            {
                VM.ReservationItemList.Add(new AddRemoveStatusVM { Name = item.Name });
            }

            return VM;
        }

        public DetailsEditUserVM GetEditUser(string userId, ClaimsPrincipal moderator)
        {
            var user = _userRepo.GetUserById(userId);
            if (user == null) return null;
            if ((!moderator.IsInRole("Admin"))
                && _userManager.IsInRoleAsync(user, "Admin").Result) return null;


            var VM = _mapper.Map<DetailsEditUserVM>(user);
            if (VM == null) return null;

            if (moderator.IsInRole("Admin"))
            {
                VM.DepartmentsList = _departmentRepo.GetDepartmentsList().Select(x => new AddRemoveStatusVM
                {
                    Id = x.Id.ToString().ToLower(),
                    Name = x.Name,
                    Status = x.AppUsers != null ? x.AppUsers.Any(y => y.AppUserId == userId) : false
                }).ToList();
            }
            else
            {
                foreach (var department in _departmentRepo.GetDepartmentsListByUser(userId))
                {
                    VM.DepartmentsList.Add(new AddRemoveStatusVM
                    {
                        Id = department.Id.ToString().ToLower(),
                        Name = department.Name,
                        Status = true
                    });
                }
            }

            if (moderator.IsInRole("Admin"))
            {
                foreach (var role in _accessConfigRepo.GetRolesList().ToList())
                {
                    if (role.Name != "User")
                    {
                        VM.RolesList.Add(new AddRemoveStatusVM {
                            Id = role.Id,
                            Name = role.Name,
                            Status = _userManager.IsInRoleAsync(user, role.Name).Result
                        });
                    }
                }
            }
            return VM;
        }

        public bool UpdateEditUser(DetailsEditUserVM input, ClaimsPrincipal moderator)
        {
            var user = _userRepo.GetUserById(input.Id);

            if ((!moderator.IsInRole("Admin"))
                && _userManager.IsInRoleAsync(user, "Admin").Result) return false;

            user.UserName = input.UserName;
            user.FullName = input.FullName;
            user.PhoneNumber = input.Phone;
            user.Email = input.Email;

            if (!_userRepo.UpdateUser(user)) return false;
            if (!string.IsNullOrEmpty(input.Password))
            {
                _userManager.RemovePasswordAsync(user);
                _userManager.AddPasswordAsync(user, input.Password);
            }

            SetActiveUser(input.IsActive, user);

            if (moderator.IsInRole("Admin"))
            {
                user.Departments = _departmentRepo.GetDepartmentsListByUser(user.Id).SelectMany(x => x.AppUsers).Where(x => x.AppUserId == user.Id).ToList();

                foreach (var department in input.DepartmentsList ?? Enumerable.Empty<AddRemoveStatusVM>())
                {
                    if (user.Departments.Any(x => x.DepartmentId.ToString() == department.Id))
                    {
                        if (!department.Status) _departmentRepo.RemoveUserFromDepartment(user.Id, department.Id);
                    }
                    else
                    {
                        if (department.Status) _departmentRepo.AddUserToDepartment(user.Id, department.Id);
                    }
                }

                foreach (var role in input.RolesList ?? Enumerable.Empty<AddRemoveStatusVM>())
                {
                    if (_userManager.IsInRoleAsync(user, role.Name).Result)
                    {
                        if (!role.Status) _accessConfigRepo.RemoveRoleFromUser(role.Name, user);
                        //_userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        if (role.Status) _accessConfigRepo.AddRoleToUser(role.Name, user);
                        //_userManager.AddToRoleAsync(user, role.Name);
                    }
                }
            }
            else
            {
                foreach (var department in input.DepartmentsList ?? Enumerable.Empty<AddRemoveStatusVM>())
                {
                    if (user.Departments.Any(x => x.Department.Name == department.Name))
                    {
                        if (!department.Status) _departmentRepo.RemoveUserFromDepartment(user.Id, department.Id);
                    }
                }
            }
            return true;
        }

        public short DeleteUser(string userId, ClaimsPrincipal moderator)
        {
            var user = _userRepo.GetUserById(userId);
            if (user == null) return -1;
            if ((!moderator.IsInRole("Admin"))
                && _userManager.IsInRoleAsync(user, "Admin").Result) return 1;

            if (user.Reservations != null && user.Reservations.Count != 0) return 2;

            foreach (var department in _departmentRepo.GetDepartmentsListByUser(userId))
            {
                _departmentRepo.RemoveUserFromDepartment(user.Id, department.Id);
            }

            if (!_userManager.DeleteAsync(user).Result.Succeeded) return -1;
            return 0;
        }

        public List<AddRemoveStatusVM> ReservationListByUser(string userId)
        {
            return _reservationRepo.GetReservationListByUser(userId)
                .Select(x => new AddRemoveStatusVM
                {
                    Id = x.Id.ToString().ToLower(),
                    Name = $"{x.Item.Name} - {x.ItemId}"
                }).ToList();
        }
    }
}
