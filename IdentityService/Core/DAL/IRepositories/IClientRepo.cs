using System.Threading.Tasks;
using IdentityService.Core.Domain;
using Shared.DAL.Interfaces;

namespace IdentityService.Core.DAL.IRepositories
{
    public interface IClientRepo : IRepository<Client>
	{
        Task<Client> FindByClientPublicIdAsync(string clientPublicId);
    }
}
