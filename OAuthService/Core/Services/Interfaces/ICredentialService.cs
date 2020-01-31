using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;
using System.Threading.Tasks;

namespace OAuthService.Core.Services.Interfaces
{
	public interface ICredentialService
	{
		Credential Get(int userId);
		Credential Get(string uuid);
		bool IsEmailExisted(string email);
		void AddUserByUserInfo(RegisterUserDto user);
		bool Logout(int userClientId, bool all = false);
		Task<Credential> CreateCredential(CredentialDto loginCredential);
		Task Register(CredentialDto credential);
        Task VerifyEmail(string token);
        AuthTokenDto Login(Credential credential, Client client = null);
    }
}
