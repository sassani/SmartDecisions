using System.Threading.Tasks;
using OAuthService.Core.Domain;
using Shared.DAL.Interfaces;

namespace OAuthService.Core.DAL.IRepositories
{
    public interface IClientRepo : IRepository<Client>
	{
        Task<Client> FindByClientPublicIdAsync(string clientPublicId);
    }
}
