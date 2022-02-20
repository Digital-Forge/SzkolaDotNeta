using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Domain.Model.ExtraModel
{
    public class EmailConfig
    {
        public string HostSmtp { get; set; }
        public bool EnableSSL { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
        public string SenderEmailPassword { get; set; }
        public string SenderName { get; set; }

    }
}
