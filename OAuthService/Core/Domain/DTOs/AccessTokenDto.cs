using Helpers;
using System;
using System.Linq;
using Shared.DTOs;

namespace IdentityService.Core.Domain.DTOs
{
	public class AccessTokenDto : AccessToken
	{
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
