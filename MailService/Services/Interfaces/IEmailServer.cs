using MailService.DTOs;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailService.Services.Interfaces
{
    public interface IEmailServer
    {
        Task SendAsync(MessageDto message);
        Task SendAsync(string fromAdd, string fromName, string toAdd, string toName, string subject, BodyBuilder body);
        Task SendVerification(string email, string token, string name = null);
        Task SendActionLink(ActionLinkDto data);
    }
}
