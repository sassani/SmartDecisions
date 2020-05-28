using System.Text.Json.Serialization;

namespace IdentityService.Core.Domain.DTOs
{
	public class AuthTokenDto
	{
		[JsonPropertyName("accessToken")]
		public string AccessToken { get; set; }

		[JsonPropertyName("refreshToken")]
		public string RefreshToken { get; set; }

		[JsonPropertyName("tokenType")]
		public string TokenType { get; set; }

		[JsonPropertyName("uid")]
		public string CredentialId { get; set; }

		public AuthTokenDto(string token, string refresh, string type, Credential credential)
		{
			AccessToken = token;
			RefreshToken = refresh;
			TokenType = type;
			CredentialId = credential.PublicId;
		}

	}
}
