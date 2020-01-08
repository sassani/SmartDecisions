using RestApi.Helpers;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace RestApi.Core.Domain.DTOs
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

		[JsonProperty(propertyName: "roles")]
		public string[] Roles { get; set; }

		public AccessTokenDto(Credential credential, int logsheetId)
		{
			CredentialId = credential.PublicId;
			LogsheetId = logsheetId;
			//Roles = user.Roles.ToArray();
			IssuedAt = DateTimeHelper.GetUnixTimestamp();
			Expiration = DateTimeHelper.GetUnixTimestamp(60);
		}
	}
}
