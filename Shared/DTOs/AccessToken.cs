using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Primitives;

namespace Shared.DTOs
{
	public class AccessToken
	{
		[JsonPropertyName("uid")]
		public string CredentialId { get; set; }

		[JsonPropertyName("lid")]
		public int LogsheetId { get; set; }

		[JsonPropertyName("iat")]
		public long IssuedAt { get; set; }

		[JsonPropertyName("exp")]
		public long Expiration { get; set; }

		[JsonPropertyName("Email")]
		public string Email { get; set; }

		[JsonPropertyName("isEmailVerified")]
		public bool IsEmailVerified { get; set; }

		[JsonPropertyName("roles")]
		public string[]? Roles { get; set; }// TODO: will change in the future

		public static AccessToken Decode(StringValues token)
		{
			AccessToken accessToken = new AccessToken();
			if (token.Count != 0)
			{
				var jsonPayload = "{" + token[0].Split("}.{")[1];
				accessToken = JsonSerializer.Deserialize<AccessToken>(jsonPayload);
			}
			return accessToken;
		}
	}
}
