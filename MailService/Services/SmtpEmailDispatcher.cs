using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailService.Services.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailService.Services
{
    public class SmtpEmailDispatcher : IMessageDispatcher
    {
        private readonly MailServerSettings config;

        public SmtpEmailDispatcher(IOptions<MailServerSettings> options)
        {
            config = options.Value;
        }

        public async Task DispatchMessageAsync(MimeMessage message)
        {
            using var client = new SmtpClient
            {
                ServerCertificateValidationCallback = (s, c, h, e) => true
            };

            await client.ConnectAsync(config.SmtpService.ServerName, config.SmtpService.PortNumber, config.SmtpService.UsingSSL);
            await client.AuthenticateAsync(config.SmtpService.Username, config.SmtpService.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
