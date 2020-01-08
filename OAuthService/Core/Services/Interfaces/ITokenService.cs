using RestApi.Core.Domain;
using RestApi.Core.Domain.DTOs;

namespace RestApi.Core.Services.Interfaces
{
	public interface ITokenService
	{
		AuthTokenDto GenerateAuthToken(Credential user, int userClientId, string refreshToken);
		bool ValidateToken(string TokenString);
		string GenerateRefreshToken(string userPublicId);
	}
}
