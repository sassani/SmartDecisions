namespace MailService
{
    public class MailServerSettings
    {
        public string Type { get; set; }
        public SmtpService SmtpService { get; set; }
        public ApiService ApiService { get; set; }
        public string DomainName { get; set; }
        public string CompanyName { get; set; }
        public string SharedApiKey { get; set; }
    }

    public class SmtpService
    {
        public string ServerName { get; set; }
        public int PortNumber { get; set; }
        public bool UsingSSL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ApiService
    {
        public string Url { get; set; }
        public string ApiKey { get; set; }
    }
}
