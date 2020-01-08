using RestApi.Core.Domain;


namespace RestApi.Core.DataServices.IRepositories
{
	public interface IClientRepo : IRepoRepo<Client>
	{
		Client FindByClientPublicId(string clientId);
	}
}
