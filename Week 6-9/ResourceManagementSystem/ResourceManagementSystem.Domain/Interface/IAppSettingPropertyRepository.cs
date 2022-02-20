using ResourceManagementSystem.Domain.Model.ExtraModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Domain.Interface
{
    public interface IAppSettingPropertyRepository
    {
        bool CanSendEmail();
        EmailConfig GetEmailConfig();
    }
}
