using System.Threading.Tasks;
using IdentityService.Core.Domain;
using Shared.DAL.Interfaces;

namespace IdentityService.Core.DAL.IRepositories
{
    public interface ICredentialRepo : IRepository<Credential>
	{
        CredentialRole[] GetRoles(Credential credential);
        void UpdateLastLogin(Credential credential);
        Task VerifyEmailAsync(string email);
        Task<Credential> FindByUidAsync(string uid);
        Task<Credential> FindByEmailAsync(string email);
        Task<bool> IsEmailExistAsync(string email);
    }

}
