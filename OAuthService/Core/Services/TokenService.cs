using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;
using OAuthService.Helpers;
using OAuthService.Core.Services.Interfaces;
using OAuthService.Extensions;
using Jose;
using Microsoft.Extensions.Options;
using System.Linq;
using System;

namespace OAuthService.Core.Services
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

        public string GetEmailVerificationToken(string email)
        {
            EmailVerificationTokenDto token = new EmailVerificationTokenDto(email);
            return JWT.Encode(token, secretKey, JwsAlgorithm.HS256);
        }

        public T ValidateDtoToken<T>(string tokenString)
        {
            T validatedDtoToken;
            try
            {
                validatedDtoToken = JWT.Decode<T>(tokenString, secretKey);
            }
            catch (Exception err)
            {
                throw err;
            }
            return validatedDtoToken;
        }

        public string GenerateRefreshToken(string userPublicId)
        {
            return userPublicId + StringHelper.GenerateRandom(37);
            //return StringHelper.StringToHash(userPublicId + StringHelper.GenerateRandom(25));
        }
    }
}
