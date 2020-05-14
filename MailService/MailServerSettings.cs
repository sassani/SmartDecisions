using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailService
{
    public class MailServerSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public bool UsingSSL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApiKey { get; set; }
        public string DomainName { get; set; }
        public string CompanyName { get; set; }
    }
}
