using OAuthService.Core.Domain;
using System.Threading.Tasks;

namespace OAuthService.Core.DataServices.IRepositories
{
	public interface ICredentialRepo : IRepoRepo<Credential>
	{
		bool IsEmailExist(string email);
		Credential FindByEmail(string email);
        CredentialRole[] GetRoles(Credential credential);
        void UpdateLastLogin(Credential credential);
        Task VerifyEmailByEmail(string email);
    }

}
