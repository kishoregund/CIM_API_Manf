using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string Admin { get; set; }
        public string SuperAdminEmail { get; set; }
        public string DBServerName { get; set; }
        public string DBUserId { get; set; }
        public string DBPassword { get; set; }
        public EmailSettings EmailSettings { get; set; }

    }

    public class EmailSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string SSL { get; set; }
        public string SMTPUser { get; set; }
        public string SMTPPassword { get; set; }
        public string DisplayName { get; set; }
        public string Subject { get; set; }

    }
}
