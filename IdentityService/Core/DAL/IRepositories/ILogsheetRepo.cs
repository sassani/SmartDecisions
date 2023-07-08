using System.Threading.Tasks;
using IdentityService.Core.Domain;
using Shared.DAL.Interfaces;

namespace IdentityService.Core.DAL.IRepositories
{
	public interface ILogsheetRepo : IRepository<Logsheet>
	{
		Task<Logsheet?> FindLogsheetByRefreshTokenAsync(string refreshToken);
		void UpdateLastTimeLogin(Logsheet logSheetId);
	}
}
