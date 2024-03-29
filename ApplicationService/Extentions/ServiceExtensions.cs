﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ApplicationService.Core.DAL;
using ApplicationService.DataBase;

namespace ApplicationService.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDb(this IServiceCollection services, AppSettingsModel config)
        {
            switch (config.DatabaseProvider)
            {
                case "MYSQL":
                    services.AddDbContext<ApiContext>(options => options
                    .UseMySql(config.DbConnectionString));
                    break;

                case "MSSQL":
                    services.AddDbContext<ApiContext>(options => options
                    .UseSqlServer(config.DbConnectionString));
                    break;

                default:
                    break;
            }
            //services.AddDbContext<ApiContext>(options => options
            //.UseMySql(config.DbConnectionString));
            //.UseSqlServer(config.DbConnectionString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureDbMySql(this IServiceCollection services, AppSettingsModel config)
        {
            services.AddDbContext<ApiContext>(options => options
            .UseMySql(config.DbConnectionString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureCors(this IServiceCollection services, AppSettingsModel config)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .WithOrigins(config.CrossUrls)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    );
            });
        }

        public static void ConfigureAuthentication(this IServiceCollection services, AppSettingsModel config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                    {
                        var jwt = new JwtSecurityToken(token);

                        return jwt;
                    },
                    ValidateAudience=false,
                    ValidateIssuer=false,
                    //ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = false,
                };
            });
        }
    }
}
