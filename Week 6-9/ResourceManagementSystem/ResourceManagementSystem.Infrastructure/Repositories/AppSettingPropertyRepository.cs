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

        public bool FakeSendEmail()
        {
            bool buff;
            try
            {
                buff = bool.Parse(_config["EmailConfig:EnableFakeSend"]);
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
                    EnableSSL = bool.Parse(_config["EmailConfig:Config:EnableSend"]),
                    HostSmtp = _config["EmailConfig:Config:HostSmtp"],
                    Port = int.Parse(_config["EmailConfig:Config:Port"]),
                    SenderEmail = _config["EmailConfig:Config:SenderEmail"],
                    SenderEmailPassword = _config["EmailConfig:Config:SenderEmailPassword"],
                    SenderName = _config["EmailConfig:Config:SenderName"]
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
