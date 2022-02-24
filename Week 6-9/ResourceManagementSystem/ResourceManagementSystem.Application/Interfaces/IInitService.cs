using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Application.Interfaces
{
    public interface IInitService
    {
        bool CanInit();

        void SetFirstAdminConfig();

        void SetConfirmEmailForFirstAdmin();
    }
}
