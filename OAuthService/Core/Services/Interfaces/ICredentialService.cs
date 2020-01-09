using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;

namespace OAuthService.Core.Services.Interfaces
{
	public interface ICredentialService
	{
		Credential Get(int userId);
		Credential Get(string uuid);
		bool CheckEmail(string email);
		void AddUserByUserInfo(RegisterUserDto user);

	}
}
