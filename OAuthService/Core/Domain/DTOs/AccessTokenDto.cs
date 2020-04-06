using Helpers;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace OAuthService.Core.Domain.DTOs
{
	public class AccessTokenDto
	{
		[JsonProperty(propertyName: "uid")]
		public string CredentialId { get; set; }

		[JsonProperty(propertyName: "lid")]
		public int LogsheetId { get; set; }

		[JsonProperty(propertyName: "iat")]
		public long IssuedAt { get; set; }

		[JsonProperty(propertyName: "exp")]
		public long Expiration { get; set; }

		[JsonProperty(propertyName: "Email")]
		public string Email { get; set; }

		[JsonProperty(propertyName: "isEmailVerified")]
		public bool IsEmailVerified { get; set; }

		[JsonProperty(propertyName: "roles")]
		public string[]? Roles { get; set; }// TODO: will change in the future

		public AccessTokenDto(Credential credential, int logsheetId)
		{
			CredentialId = credential.PublicId;
			LogsheetId = logsheetId;
			//Roles = user.Roles.ToArray();
			IssuedAt = DateTimeHelper.GetUnixTimestamp();
			Expiration = DateTimeHelper.GetUnixTimestamp(60);
			Email = credential.Email;
			IsEmailVerified = credential.IsEmailVerified;
		}
	}
}
