using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OAuthService.Core.DAL.IRepositories;
using OAuthService.Core.Domain;
using Shared.DAL;

namespace OAuthService.DataBase.Persistence.Repositories
{
	public class ClientRepo : Repository<Client>, IClientRepo
	{
		private new ApiContext context;
		public ClientRepo(ApiContext context) : base(context)
		{
			this.context = context;
		}

		public async Task<Client> FindByClientPublicIdAsync(string clientPublicId)
		{
			return await context.Client
				.Where(a => a.ClientPublicId == clientPublicId)
				.SingleOrDefaultAsync();
		}
	}
}
