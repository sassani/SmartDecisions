
using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;

namespace OAuthService.Core.Services.Interfaces
{
	public interface IClientService
	{
		Client CreateClient(string clientId, string clientSecret = null);
	}
}
