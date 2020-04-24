using Newtonsoft.Json;
using Helpers;

namespace IdentityService.Core.Domain.DTOs
{
	public class EmailVerificationTokenDto
    {
		[JsonProperty(propertyName: "eml")]
		public string Email { get; set; }

		[JsonProperty(propertyName: "iat")]
		public long IssuedAt { get; set; }

		[JsonProperty(propertyName: "exp")]
		public long Expiration { get; set; }

		public EmailVerificationTokenDto(string email)
		{
			Email = email;
			IssuedAt = DateTimeHelper.GetUnixTimestamp();
			Expiration = DateTimeHelper.GetUnixTimestamp(2*24*60);// 2 days
		}
	}
}
