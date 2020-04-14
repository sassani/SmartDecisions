
using System.Threading.Tasks;
using OAuthService.Core.Domain;

namespace OAuthService.Core.Services.Interfaces
{
	public interface IClientService
	{
		Task<Client> CreateClientAsync(string clientId, string? clientSecret = null);
	}
}
