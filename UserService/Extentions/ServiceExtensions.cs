using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Core.DAL;
using UserService.DataBase;

namespace UserService.Extensions
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
			//services.AddAuthentication(options =>
			//{
			//	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			//	options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
			//	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			//})
			//.AddJwtBearer(opt =>
			//{
			//	opt.TokenValidationParameters = new TokenValidationParameters
			//	{
			//		ClockSkew = TimeSpan.Zero,
			//		RequireSignedTokens = true,
			//		ValidateIssuer = false,
			//		ValidateAudience = false,
			//		ValidateLifetime = true,
			//		ValidateIssuerSigningKey = true,

			//		ValidIssuer = config.Token.Issuer,
			//		ValidAudience = config.Token.Audience,
			//		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Token.SecretKey))
			//	};
			//	opt.Events = new JwtBearerEvents // TODO: create a filter to handle this
			//	{
			//		OnChallenge = async context =>
			//		{
			//			// Override the response status code.
			//			context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

			//			// Emit the WWW-Authenticate header.
			//			context.Response.Headers.Append(
			//				HeaderNames.WWWAuthenticate,
			//				context.Options.Challenge);
			//			context.Response.Headers.Append(
			//				HeaderNames.ContentType,
			//				"application/json");

			//			string title = "Authorization Failed";
			//			string detail = "Authorization token is required";
			//			if (!string.IsNullOrEmpty(context.Error))title = context.Error;
			//			if (!string.IsNullOrEmpty(context.ErrorDescription))detail = context.ErrorDescription;
			//			List<Error> errors = new List<Error>();
			//			Error err = new Error
			//			{
			//				Code = "111111",
			//				Title = title,
			//				Detail = detail
			//			};
			//			errors.Add(err);
			//			await context.Response.WriteAsync(new Response(System.Net.HttpStatusCode.BadRequest, errors).ToString());

			//			context.HandleResponse();
			//		}
			//	};
			//});
		}
	}
}
