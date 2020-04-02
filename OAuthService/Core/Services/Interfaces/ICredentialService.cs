using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;
using System.Threading.Tasks;

namespace OAuthService.Core.Services.Interfaces
{
    public interface ICredentialService
    {
        Task<bool> IsEmailExistedAsync(string email);
        bool Logout(int loginId, bool all = false);
        Task RegisterAsync(CredentialDto credential);
        Task VerifyEmailAsync(string token);
        AuthTokenDto Login(Credential credential, Client? client = null);
        Task SendForgotPasswordRequestLinkAsync(string email);
        Task ChangePasswordAsync(Credential cr, string newPass);
        Task<Credential> CreateCredentialAsync(CredentialDto crDto, string uid);
        Task<Credential> CreateCredentialAsync(string uid);
        Task<Credential> CreateCredentialAsync(CredentialDto crDto);
        Task SendEmailVerificationToken(string email);
    }
}
