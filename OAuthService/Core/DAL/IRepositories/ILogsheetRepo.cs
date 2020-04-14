using System.Threading.Tasks;
using OAuthService.Core.Domain;
using Shared.DAL.Interfaces;

namespace OAuthService.Core.DAL.IRepositories
{
	public interface ILogsheetRepo : IRepository<Logsheet>
	{
		Task<Logsheet> FindLogsheetByRefreshTokenAsync(string refreshToken);
		void UpdateLastTimeLogin(Logsheet logSheetId);
	}
}
