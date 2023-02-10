using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailService.Services;
using MailService.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MailService.Extentions
{
    public static class ServiceExtentions
    {
        public static void ConfigureMessageDispatcher(this IServiceCollection services, MailServerSettings config)
        {
            switch (config.Type)
            {
                case "api":
                    services.AddTransient<IMessageDispatcher, ApiEmailDispatcher>();
                    break;
                case "smtp":
                    services.AddTransient<IMessageDispatcher, SmtpEmailDispatcher>();
                    break;
                default:
                    break;
            }
        }
    }
}
