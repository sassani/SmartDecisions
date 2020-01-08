
using RestApi.Core.Domain;
using RestApi.Core.Domain.DTOs;

namespace RestApi.Core.Services.Interfaces
{
	public interface IClientService
	{
		Client CreateClient(string clientId, string clientSecret = null);
	}
}
