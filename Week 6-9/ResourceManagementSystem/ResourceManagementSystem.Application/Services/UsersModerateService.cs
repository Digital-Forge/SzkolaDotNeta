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
                foreach (var department in _departmentRepo.GetDepartmentsList())
                {
                    if (user.Departments.Any(x => x.DepartmentId == department.Id))
                    {
                        VM.DepartmentsList.Add(new AddRemoveStatusVM
                        {
                            Id = department.Id,
                            Name = department.Name,
                            Status = true
                        });
                    }
                    else
                    {
                        VM.DepartmentsList.Add(new AddRemoveStatusVM
                        {
                            Id = department.Id,
                            Name = department.Name,
                            Status = false
                        });
                    }
                }
            }
            else
            {
                foreach (var department in user.Departments)
                {
                    VM.DepartmentsList.Add(new AddRemoveStatusVM
                    {
                        Id = department.DepartmentId,
                        Name = department.Department.Name,
                        Status = true
                    });
                }
            }

            if (moderator.IsInRole("Admin"))
            {
                foreach (var role in _accessConfigRepo.GetRolesList())
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
                foreach (var department in input.DepartmentsList)
                {
                    if (user.Departments.Any(x => x.Department.Name == department.Name))
                    {
                        if (!department.Status) _departmentRepo.RemoveUserToDepartment(user.Id, department.Id);
                    }
                    else
                    {
                        if (department.Status) _departmentRepo.AddUserToDepartment(user.Id, department.Id);
                    }
                }
            }
            else
            {
                foreach (var department in input.DepartmentsList)
                {
                    if (user.Departments.Any(x => x.Department.Name == department.Name))
                    {
                        if (!department.Status) _departmentRepo.RemoveUserToDepartment(user.Id, department.Id);
                    }
                }
            }

            foreach (var role in input.RolesList)
            {
                if (_userManager.IsInRoleAsync(user, role.Name).Result)
                {
                    if (!role.Status) _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    if (role.Status) _userManager.AddToRoleAsync(user, role.Name);
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

            if (user.Reservations.Count != 0) return 2;

            foreach (var department in user.Departments)
            {
                _departmentRepo.RemoveUserToDepartment(user.Id, department.DepartmentId);
            }

            if (!_userManager.DeleteAsync(user).Result.Succeeded) return -1;
            return 0;
        }

        public List<AddRemoveStatusVM> ReservationListByUser(string userId)
        {
            return _reservationRepo.GetReservationListByUser(userId)
                .Select(x => new AddRemoveStatusVM
                {
                    Id = x.Id,
                    Name = $"{x.Item.Name} - {x.ItemId}"
                }).ToList();
        }
    }
}
