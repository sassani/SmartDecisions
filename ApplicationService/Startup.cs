using System;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ApplicationService.Core;
using ApplicationService.DataBase;
using ApplicationService.Extensions;
using ApplicationService.Extentions;
using ApplicationService.Core.Services.Interfaces;
using ApplicationService.Core.Services;
using Shared.Storage;

namespace ApplicationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddControllers()
                .ConfigureApiBehaviorOptions(opt =>
                {
                    opt.SuppressModelStateInvalidFilter = true;
                    //opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
                });

            var appSettingSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettingsModel>(appSettingSection);
            AppSettingsModel appSettings = appSettingSection.Get<AppSettingsModel>();

            services.ConfigureDbMySql(appSettings);
            services.ConfigureAuthentication(appSettings);
            services.ConfigureCors(appSettings);
            services.AddScoped<ValidateModelAttributeFilter>();
            //services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddAutoMapper((serviceProvider, automapper) =>
            {
                automapper.AddCollectionMappers();
                automapper.UseEntityFrameworkCoreModel<ApiContext>(serviceProvider);
                automapper.AddProfile(typeof(AutoMapperProfile));
            }, typeof(ApiContext).Assembly);

            //var serviceProvider = services.BuildServiceProvider();
            services.AddScoped<IStorageService, AzureStorageBlobService>();
            services.AddTransient<IGetClaimsProvider, GetClaimsFromUser>();
            services.AddScoped<IFileService, CloudFileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                Console.WriteLine("Application Service is running ...");
            }

            //app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");

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
