using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OAuthService.Core.Services;
using OAuthService.Core.Services.Interfaces;
using OAuthService.Extensions;

namespace RestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment currentEnvironment;


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var appSettingSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettingsModel>(appSettingSection);
            AppSettingsModel appSettings = appSettingSection.Get<AppSettingsModel>();

            services.ConfigureDbMySql(appSettings);
            services.ConfigureCors(appSettings);
            services.ConfigureAuthentication(appSettings);
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc();
            services.AddHttpContextAccessor();

            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<ICredentialService, CredentialService>();
            services.AddTransient<IAuthService, AuthService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            currentEnvironment = env;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                System.Console.WriteLine("Environment setup:\t"+currentEnvironment.EnvironmentName);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
