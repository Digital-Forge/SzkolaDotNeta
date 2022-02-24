using ResourceManagementSystem.Application.Interfaces;
using ResourceManagementSystem.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagementSystem.Application.Services
{
    public class InitService : IInitService
    {
        readonly IAppSettingPropertyRepository _appSettingPropertyRepository;
        readonly IUserRepository _userRepository;

        public InitService(IAppSettingPropertyRepository appSettingPropertyRepository, IUserRepository userRepository)
        {
            _appSettingPropertyRepository = appSettingPropertyRepository;
            _userRepository = userRepository;
        }

        public bool CanInit()
        {
            return _userRepository.UsersList().Count() == 0 ? true : false;
        }

        public void SetConfirmEmailForFirstAdmin()
        {
            var buff = _userRepository.UsersList().FirstOrDefault();
            if (buff == null) Environment.Exit(1);

            buff.EmailConfirmed = true;

            _userRepository.UpdateUser(buff);
        }

        public void SetFirstAdminConfig()
        {
            
        }
    }
}
