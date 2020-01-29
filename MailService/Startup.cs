using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailService.Services;
using MailService.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MailService
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<MailServerSettings>(Configuration.GetSection("mailserversettings"));

            services.AddSingleton<IEmailServer, EmailServer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.Use(async (context, next) =>
            {
                if (context.Request.Headers.ContainsKey("APIKey"))
                {
                    if (context.Request.Headers["APIKey"].ToString() == Configuration.GetValue<String>("APIKey"))
                    {
                        await next();
                    }
                    else
                    {
                        await context.Response.WriteAsync("Invalid APIKey");
                    }
                }
                else
                {
                    await context.Response.WriteAsync("APIKey header is required!");
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
