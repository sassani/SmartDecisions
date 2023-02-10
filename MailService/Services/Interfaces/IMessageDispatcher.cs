using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;

namespace MailService.Services.Interfaces
{
    public interface IMessageDispatcher
    {
        Task DispatchMessageAsync(MimeMessage message);
    }
}
