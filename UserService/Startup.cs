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
using DecissionService.Core;
using DecissionService.DataBase;
using DecissionService.Extensions;
using DecissionService.Extentions;

namespace DecissionService
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
            services.AddScoped<ValidateModelAttributeFilter>();
            //services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddAutoMapper((serviceProvider, automapper) =>
            {
                automapper.AddCollectionMappers();
                automapper.UseEntityFrameworkCoreModel<ApiContext>(serviceProvider);
                automapper.AddProfile(typeof(AutoMapperProfile));
            }, typeof(ApiContext).Assembly);

            //var serviceProvider = services.BuildServiceProvider();

            services.AddTransient<IGetClaimsProvider, GetClaimsFromUser>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                Console.WriteLine("User Service is running ...");
            }

            //app.UseHttpsRedirection();

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
