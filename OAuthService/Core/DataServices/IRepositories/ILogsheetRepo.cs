using OAuthService.Core.Domain;
using System.Threading.Tasks;

namespace OAuthService.Core.DataServices.IRepositories
{
	public interface ILogsheetRepo : IRepoRepo<Logsheet>
	{
		Task<Logsheet> FindLogsheetByRefreshTokenAsync(string refreshToken);
		void UpdateLastTimeLogin(Logsheet logSheetId);
	}
}
