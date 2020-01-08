using RestApi.DataBase.Core.Domain;


namespace RestApi.DataBase.Persistence.Repositories.Interfaces
{
	public interface IClient : IRepo<ClientDb>
	{
		ClientDb FindByClientPublicId(string clientId);
	}
}
