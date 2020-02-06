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
        void Send(string fromAdd, string fromName, string toAdd, string toName, string subject, BodyBuilder body);
        void SendVerification(string email, string token, string name = null);
        Task SendActionLink(ActionLinkDto data);
    }
}
