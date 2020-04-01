#nullable disable
using OAuthService.Core;

namespace OAuthService.Extensions
{
	public class AppSettingsModel
	{
		public string DbConnectionString { get; set; }
		public string BaseUrl { get; set; }
		public string[] CrossUrls { get; set; }
		public Token Token { get; set; }
		public BaseClient BaseClient { get; set; }
		public BaseAdmin BaseAdmin { get; set; }
		public ServicesApiKeys ServicesApiKeys { get; set; }
		public RedirectUrls RedirectUrls { get; set; }
	}

	public class Token
	{
		public string SecretKey { get; set; }
		public string Issuer { get; set; }
		public string Audience { get; set; }
		public int DurationMin { get; set; }
	}

	public class BaseClient
	{
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string Name { get; set; }
		public AppEnums.ClientType  Type { get; set; }
	}

	public class BaseAdmin
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}

	public class ServicesApiKeys
	{
		public string MailService { get; set; }
	}

	public class RedirectUrls
	{
		public string ForgotPasswordChange { get; set; }
		public string EmailVerification { get; set; }
	}
}