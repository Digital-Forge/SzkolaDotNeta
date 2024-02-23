using Application.Attributes;
using Application.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

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

        public Guid CreateUser(IUserService.CreateUserModel user)
        {
            var newUser = new UserData()
            {
                Email = user.Email,
                UserName = user.UserName,
                NormalizedUserName = user.UserName.ToUpper(),
                Active = true,
                CreateBy = Guid.Empty,
                UpdateBy = Guid.Empty,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                EntityStatus = Domain.Utils.EntityStatus.Use,
                EmailConfirmed = true
            };

            var result = _userManager.CreateAsync(newUser, user.Password).Result;
            if (result.Succeeded) return newUser.Id;
            throw new ArgumentException();
        }

        public IUserService.UserPanelAccessModel GetPanelAccess()
        {
            var isAdmin = _roleRepository.CheckUserHasRole(Constans.Constans.RoleName.Administration).Result;

            if (isAdmin) return new IUserService.UserPanelAccessModel() { 
                Admin = true, 
                PickUpPoint = true 
            };

            return new IUserService.UserPanelAccessModel()
            {
                Admin = false,
                PickUpPoint = _roleRepository.CheckUserHasRole(Constans.Constans.RoleName.PickUpPoint).Result
            };
        }
    }
}
