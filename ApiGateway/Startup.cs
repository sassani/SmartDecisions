using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using ApiGateway.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Shared.Response;
using Shared.Middlewares;
using Shared;
using System.IO;

namespace ApiGateway
{
    public class Startup
    {
        private AppSettingsModel config;
        public Startup(IConfiguration configuration) =>  Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            var path = Directory.GetCurrentDirectory();

            var appSettingSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettingsModel>(appSettingSection);
            AppSettingsModel appSettings = appSettingSection.Get<AppSettingsModel>();
            config = appSettings;

            services.ConfigureCors(appSettings);
            services.ConfigureAuthentication(appSettings);
            services.AddOcelot();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                Console.WriteLine("API Gateway is running ...");
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Use(async (context, next) =>
            {
                if (!context.Request.Headers.ContainsKey(GLOBAL_CONSTANTS.SHARED_API_KEY_HEADER_NAME))
                {
                    context.Request.Headers.Add(GLOBAL_CONSTANTS.SHARED_API_KEY_HEADER_NAME, config.SharedApiKey);
                }
                else
                {
                    context.Request.Headers[GLOBAL_CONSTANTS.SHARED_API_KEY_HEADER_NAME] = config.SharedApiKey;
                }
                await next();
            });

            var ocelotConfig = new OcelotPipelineConfiguration
            {
                AuthenticationMiddleware = async (context, next) =>
                {
                    if (context.DownstreamReRoute.IsAuthenticated)
                    {
                        var tokenValidationParams = new TokenValidationParameters
                        {
                            ClockSkew = TimeSpan.Zero,
                            RequireSignedTokens = true,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = config.Token.Issuer,
                            ValidAudience = config.Token.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Token.SecretKey))
                        };

                        SecurityToken validatedToken;
                        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                        try
                        {
                            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"];
                            if (authorizationHeader.Count == 0) throw new Exception("Authorization token is required");
                            var token = authorizationHeader[0].Split(" ")[1];
                            var user = handler.ValidateToken(token, tokenValidationParams, out validatedToken);
                            await next.Invoke();
                        }
                        catch (Exception err)
                        {
                            var res = context.HttpContext.Response;
                            res.StatusCode = (int)HttpStatusCode.Unauthorized;
                            res.Headers.Append(HeaderNames.ContentType, "application/json");
                            Error error = new Error
                            {
                                Code = "00000000",
                                Title = "Authorization Failed",
                                Detail = err.Message
                            };

                            await res.WriteAsync(new Response(HttpStatusCode.Unauthorized, error).ToString());
                        }
                    }
                    else
                    {
                        await next.Invoke();
                    }
                }
            };

            app.UseOcelot(ocelotConfig).Wait();

        }
    }
}