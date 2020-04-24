
using System.Threading.Tasks;
using IdentityService.Core.Domain;

namespace IdentityService.Core.Services.Interfaces
{
	public interface IClientService
	{
		Task<Client> CreateClientAsync(string clientId, string? clientSecret = null);
	}
}
