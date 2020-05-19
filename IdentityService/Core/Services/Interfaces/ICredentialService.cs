using IdentityService.Core.Domain;
using IdentityService.Core.Domain.DTOs;
using System.Threading.Tasks;

namespace IdentityService.Core.Services.Interfaces
{
    public interface ICredentialService
    {
        Task<bool> IsEmailExistedAsync(string email);
        Task RegisterAsync(CredentialDto credential);
        Task VerifyEmailAsync(string token);
        Task SendForgotPasswordRequestLinkAsync(string email);
        //Task ChangePasswordAsync(Credential cr, string newPass);
        Task ChangePasswordAsync(CredentialDto crdt, string uid);
        Task<Credential> CreateCredentialAsync(CredentialDto crDto, string uid);
        Task<Credential> CreateCredentialAsync(string uid);
        Task<Credential> CreateCredentialAsync(CredentialDto crDto);
        Task SendEmailVerificationTokenAsync(string email);
        Task<AuthTokenDto> LoginAsync(Credential credential, Client? client = null);
        Task<bool> LogoutAsync(int LogintId, bool all = false);
    }
}
