using RestApi.Core.Domain;
using RestApi.Core.DataServices.IRepositories;

namespace RestApi.DataBase.Persistence.Repositories
{
	public class CredentialRoleRepo : Repo<CredentialRole>, ICredentialRoleRepo
	{
		public CredentialRoleRepo(ApiContext context) : base(context)
		{
		}
	}
}
