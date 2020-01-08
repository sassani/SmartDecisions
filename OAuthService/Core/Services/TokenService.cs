using RestApi.Core.Domain;
using RestApi.Core.Domain.DTOs;
using RestApi.Helpers;
using RestApi.Core.Services.Interfaces;
using RestApi.Extensions;
using Jose;
using Microsoft.Extensions.Options;
using System.Linq;

namespace RestApi.Core.Services
{
	public class TokenService : ITokenService
	{
		private readonly IOptions<AppSettingsModel> config;
		private readonly byte[] secretKey;
		public TokenService(IOptions<AppSettingsModel> config)
		{
			this.config = config;
			secretKey = config.Value.Token.SecretKey.Select(x => (byte)x).ToArray();
		}

		public AuthTokenDto GenerateAuthToken(Credential credential, int userClientId, string refreshToken)
		{
			AccessTokenDto accessToken = new AccessTokenDto(credential, userClientId);
			string signedAccessToken = JWT.Encode(accessToken, secretKey, JwsAlgorithm.HS256);
			return new AuthTokenDto(signedAccessToken, refreshToken, "bearer", credential);
		}

		public bool ValidateToken(string accessToken)
		{
			string tokenString = accessToken.Split(' ')[1];
			var temp = JWT.Decode<AccessTokenDto>(tokenString, secretKey);
			return true;
		}

		public string GenerateRefreshToken(string userPublicId)
		{
			return userPublicId + StringHelper.GenerateRandom(37);
			//return StringHelper.StringToHash(userPublicId + StringHelper.GenerateRandom(25));
		}
	}
}
