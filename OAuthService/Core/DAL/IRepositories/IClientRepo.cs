using OAuthService.Core.Domain;
using System.Threading.Tasks;

namespace OAuthService.Core.DAL.IRepositories
{
	public interface IClientRepo : IRepoRepo<Client>
	{
		Client FindByClientPublicId(string clientId);
        Task<Client> FindByClientPublicIdAsync(string clientPublicId);
    }
}
