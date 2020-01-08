using RestApi.Core.Domain;
using RestApi.Core.Domain.DTOs;

namespace RestApi.Core.Services.Interfaces
{
	public interface IAuthService
	{
		bool Authenticate(LoginCredentialDto loginUser, ref Credential user);
		AuthTokenDto Login(Client client, Credential user);
		bool Logout(int userClientId, bool all = false);
	}
}
