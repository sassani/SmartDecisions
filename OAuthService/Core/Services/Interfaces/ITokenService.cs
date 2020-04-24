using IdentityService.Core.Domain;
using IdentityService.Core.Domain.DTOs;

namespace IdentityService.Core.Services.Interfaces
{
	public interface ITokenService
	{
		AuthTokenDto GenerateAuthToken(Credential user, int userClientId, string refreshToken);
		string GenerateRefreshToken(string userPublicId);
		string EmailVerificationToken(string email);
        string ForgotPasswordRequestToken(string email);
        T ValidateDtoToken<T>(string token);
    }
}
