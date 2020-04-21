#nullable disable
using UserService.Core;

namespace UserService.Extensions
{
	public class AppSettingsModel
	{
		public string DbConnectionString { get; set; }
		public string BaseUrl { get; set; }
		public string[] CrossUrls { get; set; }
		public Token Token { get; set; }
	}

	public class Token
	{
		public string SecretKey { get; set; }
		public string Issuer { get; set; }
		public string Audience { get; set; }
		public int DurationMin { get; set; }
	}
}