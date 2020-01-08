using RestApi.Core.Entities;
using RestApi.DataBase.Core.Domain;


namespace RestApi.DataBase.Persistence.Repositories.Interfaces
{
	public interface ILogsheet : IRepo<Logsheet>
	{
		//void Login(UserClientDb userClient);
		//void Logout(UserClientDb userClient);
		Logsheet FindByRefreshToken(string refreshToken);
	}
}
