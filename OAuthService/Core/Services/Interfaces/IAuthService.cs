using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;

namespace OAuthService.Core.Services.Interfaces
{
	public interface IAuthService
	{
		bool Authenticate(LoginCredentialDto loginUser, ref Credential user);
		AuthTokenDto Login(Client client, Credential user);
		bool Logout(int userClientId, bool all = false);
	}
}
