using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using IdentityService.Core.DAL;
using IdentityService.Core.Domain.DTOs;
using IdentityService.Core.Domain.DTOs.Validators;
using IdentityService.DataBase;
using IdentityService.DataBase.Persistence;
using Shared.Response;

namespace IdentityService.Extensions
{
	public static class ServiceExtensions
	{
		public static void ConfigureDb(this IServiceCollection services, AppSettingsModel config)
		{

			services.AddDbContext<ApiContext>(options => options
			//.UseMySql(config.DbConnectionString));
			.UseSqlServer(config.DbConnectionString));
			services.AddScoped<IUnitOfWork, UnitOfWork>();
		}
		public static void AddValidators(this IServiceCollection services)
		{
			services.AddTransient<IValidator<CredentialDto>, CredentialDtoValidator>();
		}

		public static void ConfigureDbMySql(this IServiceCollection services, AppSettingsModel config)
		{
			services.AddDbContext<ApiContext>(options => options
			///.UseLazyLoadingProxies()
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
					.AllowAnyMethod()
					.AllowAnyHeader()
					);
			});
		}

		public static void ConfigureAuthentication(this IServiceCollection services, AppSettingsModel config)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(opt =>
			{
				opt.TokenValidationParameters = new TokenValidationParameters
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
				opt.Events = new JwtBearerEvents // TODO: create a filter to handle this
				{
					OnChallenge = async context =>
					{
						// Override the response status code.
						context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

						// Emit the WWW-Authenticate header.
						context.Response.Headers.Append(
							HeaderNames.WWWAuthenticate,
							context.Options.Challenge);
						context.Response.Headers.Append(
							HeaderNames.ContentType,
							"application/json");

						string title = "Authorization Failed";
						string detail = "Authorization token is required";
						if (!string.IsNullOrEmpty(context.Error))title = context.Error;
						if (!string.IsNullOrEmpty(context.ErrorDescription))detail = context.ErrorDescription;
						List<Error> errors = new List<Error>();
						Error err = new Error
						{
							Code = "111111",
							Title = title,
							Detail = detail
						};
						errors.Add(err);
						await context.Response.WriteAsync(new Response(System.Net.HttpStatusCode.BadRequest, errors).ToString());

						context.HandleResponse();
					}
				};
			});
		}
	}
}
