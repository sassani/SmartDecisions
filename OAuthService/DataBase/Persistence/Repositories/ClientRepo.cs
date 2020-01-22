using OAuthService.Core.Domain;
using OAuthService.Core.DataServices.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthService.DataBase.Persistence.Repositories
{
	public class ClientRepo : Repo<Client>, IClientRepo
	{
		private new ApiContext context;
		public ClientRepo(ApiContext context) : base(context)
		{
			this.context = context;
		}

		public Client FindByClientPublicId(string publicId)
		{
			return context.Client
				.Where(a => a.ClientPublicId == publicId)
				.SingleOrDefault();
		}

		public Task<Client> FindByClientPublicIdAsync(string clientPublicId)
		{
			return Task.Run(() =>
			{
				return context.Client
				.Where(a => a.ClientPublicId == clientPublicId)
				.SingleOrDefaultAsync();
			});
		}
	}
}
