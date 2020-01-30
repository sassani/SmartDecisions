using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;

namespace OAuthService.Core.Services.Interfaces
{
	public interface ITokenService
	{
		AuthTokenDto GenerateAuthToken(Credential user, int userClientId, string refreshToken);
		EmailVerificationTokenDto ValidateToken(string TokenString);
		string GenerateRefreshToken(string userPublicId);
		string GetEmailVerificationToken(string email);
	}
}
