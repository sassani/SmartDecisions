using Newtonsoft.Json;
using OAuthService.Helpers;

namespace OAuthService.Core.Domain.DTOs
{
    public class ForgotPasswordRequestTokenDto
    {
		[JsonProperty(propertyName: "eml")]
		public string Email { get; set; }

		[JsonProperty(propertyName: "iat")]
		public long IssuedAt { get; set; }

		[JsonProperty(propertyName: "exp")]
		public long Expiration { get; set; }

		public ForgotPasswordRequestTokenDto(string email)
		{
			Email = email;
			IssuedAt = DateTimeHelper.GetUnixTimestamp();
			Expiration = DateTimeHelper.GetUnixTimestamp(1 * 1 * 20);// 20 min
		}
	}
}
