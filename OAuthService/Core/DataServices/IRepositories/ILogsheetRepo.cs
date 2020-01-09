using OAuthService.Core.Domain;

namespace OAuthService.Core.DataServices.IRepositories
{
	public interface ILogsheetRepo : IRepoRepo<Logsheet>
	{
		//void Login(UserClientDb userClient);
		//void Logout(UserClientDb userClient);
		Logsheet FindByRefreshToken(string refreshToken);
	}
}
