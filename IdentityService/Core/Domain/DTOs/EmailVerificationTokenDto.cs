using System.Text.Json.Serialization;
using Helpers;

namespace IdentityService.Core.Domain.DTOs
{
	public class EmailVerificationTokenDto
    {
		[JsonPropertyName("eml")]
		public string Email { get; set; }

		[JsonPropertyName("iat")]
		public long IssuedAt { get; set; }

		[JsonPropertyName("exp")]
		public long Expiration { get; set; }

		public EmailVerificationTokenDto(string email)
		{
			Email = email;
			IssuedAt = DateTimeHelper.GetUnixTimestamp();
			Expiration = DateTimeHelper.GetUnixTimestamp(2*24*60);// 2 days
		}
	}
}
