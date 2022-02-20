using Microsoft.Extensions.Configuration;
using ResourceManagementSystem.Domain.Model.ExtraModel;
using ResourceManagementSystem.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagementSystem.Infrastructure.Repositories
{
    public class AppSettingPropertyRepository : IAppSettingPropertyRepository
    {
        private readonly IConfiguration _config;

        public AppSettingPropertyRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        public bool CanSendEmail()
        {
            bool buff;
            try
            {
                buff = bool.Parse(_config["EmailSettings:EnableSend"]);
            }
            catch
            {
                buff = false;
            }
            return buff;
        }

        public EmailConfig GetEmailConfig()
        {
            EmailConfig buff = null;

            try
            {
                buff = new EmailConfig
                {
                    EnableSSL = bool.Parse(_config["EmailSettings:EnableSend"]),
                    HostSmtp = _config["EmailSettings:Setting:HostSmtp"],
                    Port = int.Parse(_config["EmailSettings:Setting:Port"]),
                    SenderEmail = _config["EmailSettings:Setting:SenderEmail"],
                    SenderEmailPassword = _config["EmailSettings:Setting:SenderEmailPassword"],
                    SenderName = _config["EmailSettings:Setting:SenderName"]
                };
            }
            catch
            {
                buff = null;
            }
            return buff;
        }
    }
}
