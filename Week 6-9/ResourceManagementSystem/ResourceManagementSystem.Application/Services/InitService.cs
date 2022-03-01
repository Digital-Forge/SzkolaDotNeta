using Microsoft.AspNetCore.Identity;
using ResourceManagementSystem.Application.Interfaces;
using ResourceManagementSystem.Application.ViewModel.Init;
using ResourceManagementSystem.Domain.Interface;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ResourceManagementSystem.Application.Services
{
    public class InitService : IInitService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppSettingPropertyRepository _appSettingPropertyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccessConfigRepository _accessConfigRepository;

        public InitService(IAppSettingPropertyRepository appSettingPropertyRepository, 
            IUserRepository userRepository, 
            UserManager<AppUser> userManager,
            IAccessConfigRepository accessConfigRepository)
        {
            _userManager = userManager;
            _appSettingPropertyRepository = appSettingPropertyRepository;
            _userRepository = userRepository;
            _accessConfigRepository = accessConfigRepository;
        }

        public bool CanInit()
        {
            return _userRepository.GetUsersList().Count() == 0 ? true : false;
        }

        public void SetConfirmEmailForFirstAdmin()
        {
            var buff = _userRepository.GetUsersList().FirstOrDefault();
            if (buff == null) Environment.Exit(1);

            buff.EmailConfirmed = true;

            _userRepository.UpdateUser(buff);
        }

        public void SetDefaultConfig()
        {
            _accessConfigRepository.AddRole("UserModerator");
            _accessConfigRepository.AddRole("ItemModerator");
            _accessConfigRepository.AddRole("DepartmentModerator");
            _accessConfigRepository.AddRole("PickupPoint");
            _accessConfigRepository.AddRole("Admin");
        }

        public void SetFirstAdminConfig(ClaimsPrincipal user)
        {
            var userBuff = _userManager.GetUserAsync(user).Result;
            _accessConfigRepository.AddRoleToUser("Admin", userBuff);
            
        }

        public bool SetFirstAdminData(AdminAccountVM model)
        {
            var account = _userRepository.GetUsersList().Where(x => x.UserName.ToLower() == "admin").FirstOrDefault();

            if (account == null) return false;

            if (!_userManager.SetEmailAsync(account, model.Email).Result.Succeeded) return false;
            if (!_userManager.SetUserNameAsync(account, model.Username).Result.Succeeded) return false;
            if (!_userManager.ChangePasswordAsync(account, "AdminAccountInit123", model.Password).Result.Succeeded) return false;

            account.FullName = model.FullName;
            account.isActive = true;
            account.isFirstAccess = true;
            account.EmailConfirmed = true;

            return _userRepository.UpdateUser(account);
        }
    }
}
