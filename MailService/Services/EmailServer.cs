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
using MailService.Templates;

namespace MailService.Services
{
    public class EmailServer : IEmailServer
    {
        private readonly MailServerSettings config;
        private readonly IMessageDispatcher messageDispatcher;
        public EmailServer(IOptions<MailServerSettings> options, IMessageDispatcher messageDispatcher)
        {
            config = options.Value;
            this.messageDispatcher = messageDispatcher;
        }

        public async Task SendAsync(string fromAdd,
                      string fromName,
                      string toAdd,
                      string toName,
                      string subject,
                      BodyBuilder body)
        {
            var mMessage = new MimeMessage();
            mMessage.From.Add(new MailboxAddress(fromName, fromAdd));
            mMessage.To.Add(new MailboxAddress(toName, toAdd));
            mMessage.Subject = subject;
            mMessage.Body = body.ToMessageBody();
            await messageDispatcher.DispatchMessageAsync(mMessage);
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
            await messageDispatcher.DispatchMessageAsync(mMessage);
        }

        public async Task SendVerification(string email, string token, string name = null)
        {
            string subject = $"Welcome {(name != null ? name : "To FSS")} Please Verify your Email";

            var body = TemplateManager.CreateBodyFromTemplate("Templates/verificationTemplate.html", new Dictionary<string, string>() {
                { "email" , email },
                { "verifingToken" , token }
            });

            await SendAsync("noreply@ardavansassani.com", "Fast Smart Solutions", email, name, subject, body);

        }

        public async Task SendActionLink(ActionLinkDto data)
        {
            string subject = $"{data.Title}";

            var body = TemplateManager.CreateBodyFromTemplate("Templates/ActionLinkTemplate.html", new Dictionary<string, string>() {
                { "title" , data.Title },
                { "description" , data.Description },
                { "url" , data.Url },
                { "label" , data.Label },
            });

            await SendAsync($"noreply@{config.DomainName}", config.CompanyName, data.Email, null, subject, body);
        }
    }
}
