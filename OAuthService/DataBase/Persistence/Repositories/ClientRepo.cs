using RestApi.Core.Domain;
using RestApi.Core.DataServices.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RestApi.DataBase.Persistence.Repositories
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
	}
}
