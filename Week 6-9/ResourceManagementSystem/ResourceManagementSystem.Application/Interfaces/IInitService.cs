using ResourceManagementSystem.Application.ViewModel.Init;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ResourceManagementSystem.Application.Interfaces
{
    public interface IInitService
    {
        bool CanInit();

        bool SetFirstAdminData(AdminAccountVM model);

        void SetDefaultConfig();

        void SetFirstAdminConfig(ClaimsPrincipal user);

        void SetConfirmEmailForFirstAdmin();
    }
}
