using RestApi.Core.Domain;


namespace RestApi.Core.DataServices.IRepositories
{
	public interface ICredentialRepo : IRepoRepo<Credential>
	{
		bool IsEmailExist(string email);
		Credential FindByEmail(string email);
        CredentialRole[] GetRoles(Credential credential);
        void UpdateLastLogin(int credentialId);
	}

}
