using OAuthService.Core.DAL.IRepositories;
using OAuthService.Core.Domain;
using Shared.DAL;

namespace OAuthService.DataBase.Persistence.Repositories
{
	public class CredentialRoleRepo : Repository<CredentialRole>, ICredentialRoleRepo
	{
		public CredentialRoleRepo(ApiContext context) : base(context)
		{
		}
	}
}
