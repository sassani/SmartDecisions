
using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;
using System.Threading.Tasks;

namespace OAuthService.Core.Services.Interfaces
{
	public interface IClientService
	{
		Client CreateClient(string clientId, string? clientSecret = null);
		Task<Client> CreateClientAsync(string clientId, string? clientSecret = null);
	}
}
