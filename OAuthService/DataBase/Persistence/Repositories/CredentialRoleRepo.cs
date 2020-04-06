using OAuthService.Core.Domain;
using OAuthService.Core.DAL.IRepositories;

namespace OAuthService.DataBase.Persistence.Repositories
{
	public class CredentialRoleRepo : Repo<CredentialRole>, ICredentialRoleRepo
	{
		public CredentialRoleRepo(ApiContext context) : base(context)
		{
		}
	}
}
