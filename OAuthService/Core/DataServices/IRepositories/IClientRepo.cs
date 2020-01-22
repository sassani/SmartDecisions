using OAuthService.Core.Domain;
using System.Threading.Tasks;

namespace OAuthService.Core.DataServices.IRepositories
{
	public interface IClientRepo : IRepoRepo<Client>
	{
		Client FindByClientPublicId(string clientId);
        Task<Client> FindByClientPublicIdAsync(string clientPublicId);
    }
}
