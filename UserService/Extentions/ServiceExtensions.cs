﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using DecissionService.Core.DAL;
using DecissionService.DataBase;

namespace DecissionService.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddValidators(this IServiceCollection services)
        {
            //services.AddTransient<IValidator<CredentialDto>, CredentialDtoValidator>();
        }

        public static void ConfigureDbMySql(this IServiceCollection services, AppSettingsModel config)
        {
            services.AddDbContext<ApiContext>(options => options
            .UseMySql(config.DbConnectionString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureCors(this IServiceCollection services, AppSettingsModel config)
        {
            //services.AddCors(options =>
            //{
            //	options.AddPolicy("CorsPolicy",
            //		builder => builder
            //		//.WithOrigins(config.CrossUrls)
            //		.WithOrigins("http://localhost:4200")
            //		.WithHeaders("*")
            //		//.AllowAnyOrigin() // TODO: change this before deployment!
            //		.AllowAnyMethod()
            //		//.AllowAnyHeader()
            //		//.WithMethods("GET","POST","DELETE","OPTIONS")
            //		);
            //});
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