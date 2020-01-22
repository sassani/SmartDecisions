using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;
using System.Threading.Tasks;

namespace OAuthService.Core.Services.Interfaces
{
	public interface ICredentialService
	{
		Credential Get(int userId);
		Credential Get(string uuid);
		bool CheckEmail(string email);
		void AddUserByUserInfo(RegisterUserDto user);
		AuthTokenDto Login(Client client, Credential user);
		bool Logout(int userClientId, bool all = false);
		Task<Credential> CreateCredential(LoginCredentialDto loginCredential);

	}
}
