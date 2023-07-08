using IdentityService.Core.Domain;
using IdentityService.Core.Domain.DTOs;
using System.Threading.Tasks;

namespace IdentityService.Core.Services.Interfaces
{
	public interface ITokenService
	{
		Task<AuthTokenDto> GenerateAuthToken(Credential user, int userClientId, string refreshToken);
        Task<string> GetPublicKey();

        string GenerateRefreshToken(string userPublicId);
		string EmailVerificationToken(string email);
        string ForgotPasswordRequestToken(string email);
        T ValidateDtoToken<T>(string token);
    }
}
