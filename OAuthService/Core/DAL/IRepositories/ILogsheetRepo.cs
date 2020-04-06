using OAuthService.Core.Domain;
using System.Threading.Tasks;

namespace OAuthService.Core.DAL.IRepositories
{
	public interface ILogsheetRepo : IRepoRepo<Logsheet>
	{
		Task<Logsheet> FindLogsheetByRefreshTokenAsync(string refreshToken);
		void UpdateLastTimeLogin(Logsheet logSheetId);
	}
}
