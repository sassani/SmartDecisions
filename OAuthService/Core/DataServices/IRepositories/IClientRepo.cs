using OAuthService.Core.Domain;


namespace OAuthService.Core.DataServices.IRepositories
{
	public interface IClientRepo : IRepoRepo<Client>
	{
		Client FindByClientPublicId(string clientId);
	}
}
