using System;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Net;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MailService.Services.Interfaces;
using System.Threading.Tasks;
using MailService.DTOs;

namespace MailService.Services
{
    public class EmailServer : IEmailServer
    {
        private readonly MailServerSettings config;
        public EmailServer(IOptions<MailServerSettings> options)
        {
            config = options.Value;
        }

        public async Task SendAsync(string fromAdd,
                      string fromName,
                      string toAdd,
                      string toName,
                      string subject,
                      BodyBuilder body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromAdd));
            message.To.Add(new MailboxAddress(toName, toAdd));
            message.Subject = subject;
            message.Body = body.ToMessageBody();
            await SendMessageAsync(message);
        }

        public async Task SendAsync(MessageDto message)
        {
            var mMessage = new MimeMessage();
            mMessage.From.Add(new MailboxAddress(message.FromName, message.FromAddress));
            mMessage.To.Add(new MailboxAddress(message.ToName, message.ToAddress));
            mMessage.Subject = message.Subject;
            mMessage.Body = new TextPart("plain")
            {
                Text = message.Body
            };
            await SendMessageAsync(mMessage);
        }

        public async Task SendVerification(string email, string token, string name = null)
        {
            string subject = $"Welcome {(name != null ? name : "To FSS")} Please Verify your Email";

            var body = CreateBodyFromTemplate("Templates/verificationTemplate.html", new Dictionary<string, string>() {
                { "email" , email },
                { "verifingToken" , token }
            });

            await SendAsync("noreply@ardavansassani.com", "Fast Smart Solutions", email, name, subject, body);

        }

        public async Task SendActionLink(ActionLinkDto data)
        {
            string subject = $"{data.Title}";

            var body = CreateBodyFromTemplate("Templates/ActionLinkTemplate.html", new Dictionary<string, string>() {
                { "title" , data.Title },
                { "description" , data.Description },
                { "url" , data.Url },
                { "label" , data.Label },
            });

            await SendAsync($"noreply@{config.DomainName}", config.CompanyName, data.Email, null, subject, body);
        }

        private async Task SendMessageAsync(MimeMessage message)
        {
            using var client = new SmtpClient
            {
                ServerCertificateValidationCallback = (s, c, h, e) => true
            };

            await client.ConnectAsync(config.SmtpServer, config.SmtpPort, config.UsingSSL);
            await client.AuthenticateAsync(config.Username, config.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private BodyBuilder CreateBodyFromTemplate(string filePath, Dictionary<string, string> parameters)
        {
            BodyBuilder template = new BodyBuilder();

            using (StreamReader SourceReader = File.OpenText(filePath))
            {
                string rawBody = SourceReader.ReadToEnd();
                foreach (var param in parameters)
                {
                    rawBody = rawBody.Replace("{" + param.Key + "}", param.Value);
                }
                template.HtmlBody = rawBody;
            }
            return template;
        }

    }
}
