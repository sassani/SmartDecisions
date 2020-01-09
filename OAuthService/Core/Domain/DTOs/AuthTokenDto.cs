using Newtonsoft.Json;
using System.Linq;

namespace OAuthService.Core.Domain.DTOs
{
	public class AuthTokenDto
	{
		[JsonProperty(propertyName: "accessToken")]
		public string AccessToken { get; set; }

		[JsonProperty(propertyName: "refreshToken")]
		public string RefreshToken { get; set; }

		[JsonProperty(propertyName: "tokenType")]
		public string TokenType { get; set; }

		[JsonProperty(propertyName: "uid")]
		public string CredentialId { get; set; }

		[JsonProperty(propertyName: "roles")]
		public string[] CredentialRoles { get; set; }

		public AuthTokenDto(string token, string refresh, string type, Credential user)
		{
			AccessToken = token;
			RefreshToken = refresh;
			TokenType = type;
			CredentialId = user.PublicId;
			//UserRoles = user.Roles.ToArray();
		}

	}
}
