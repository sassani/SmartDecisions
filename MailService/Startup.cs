using MailService.Extentions;
using MailService.Services;
using MailService.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Middlewares.Extensions;

namespace MailService
{
    public class Startup
    {
        private readonly MailServerSettings mailServerSettings;
        public Startup(IConfiguration config)
        {
            Configuration = config;
            mailServerSettings = config.GetSection("mailserversettings").Get<MailServerSettings>();
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MailServerSettings>(Configuration.GetSection("mailserversettings"));            
            services.AddControllers();
            services.AddSingleton<IEmailServer, EmailServer>();
            services.ConfigureMessageDispatcher(mailServerSettings);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseRequestSharedApiKey(mailServerSettings.SharedApiKey);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
