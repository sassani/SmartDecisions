using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;

namespace OAuthService.Core.Services.Interfaces
{
	public interface ITokenService
	{
		AuthTokenDto GenerateAuthToken(Credential user, int userClientId, string refreshToken);
		string GenerateRefreshToken(string userPublicId);
		string GetEmailVerificationToken(string email);
        string GetForgotPasswordRequestToken(string email);
        T ValidateDtoToken<T>(string token);
    }
}
