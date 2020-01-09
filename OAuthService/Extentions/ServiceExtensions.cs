using OAuthService.DataBase;
using OAuthService.DataBase.Persistence;
using OAuthService.Core.DataServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OAuthService.Extensions
{
	public static class ServiceExtensions
	{
		//public static void ConfigureDb(this IServiceCollection services, AppSettings config)
		//{

		//	services.AddDbContext<ApiContext>(options => options
		//	///.UseLazyLoadingProxies()
		//	.UseSqlServer(config.DbConnection));
		//	services.AddScoped<IUnitOfWork, UnitOfWork>();
		//}

		public static void ConfigureDbMySql(this IServiceCollection services, AppSettingsModel config)
		{
			var server = config.DbConfiguration.Server;
			var db = config.DbConfiguration.Database;
			var port = config.DbConfiguration.Port;
            var uid = config.DbConfiguration.UserId;
			var password = config.DbConfiguration.Password;
			services.AddDbContext<ApiContext>(options => options
			///.UseLazyLoadingProxies()
			.UseMySql($"Server={server}; port={port}; Database={db}; uid={uid}; password={password};"));
			services.AddScoped<IUnitOfWork, UnitOfWork>();
		}

		public static void ConfigureCors(this IServiceCollection services, AppSettingsModel config)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
					builder => builder
					//.WithOrigins(config.CrossUrls)
					.AllowAnyOrigin() // TODO: change this before deployment!
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowCredentials());
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
					RequireSignedTokens = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,

					ValidIssuer = config.Token.Issuer,
					ValidAudience = config.Token.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Token.SecretKey))
				};
			});
		}
	}
}
