using IdentityService.Core.DAL.IRepositories;
using IdentityService.Core.Domain;
using Shared.DAL;

namespace IdentityService.DataBase.Persistence.Repositories
{
	public class CredentialRoleRepo : Repository<CredentialRole>, ICredentialRoleRepo
	{
		public CredentialRoleRepo(ApiContext context) : base(context)
		{
		}
	}
}
