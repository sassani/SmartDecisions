using RestApi.Core.Domain;

namespace RestApi.Core.DataServices.IRepositories
{
	public interface ILogsheetRepo : IRepoRepo<Logsheet>
	{
		//void Login(UserClientDb userClient);
		//void Logout(UserClientDb userClient);
		Logsheet FindByRefreshToken(string refreshToken);
	}
}
