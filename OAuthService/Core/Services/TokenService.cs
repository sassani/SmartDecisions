using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;
using OAuthService.Helpers;
using OAuthService.Core.Services.Interfaces;
using OAuthService.Extensions;
using Jose;
using Microsoft.Extensions.Options;
using System.Linq;
using System;
using System.Numerics;

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

        public string EmailVerificationToken(string email)
        {
            EmailVerificationTokenDto token = new EmailVerificationTokenDto(email);
            return JWT.Encode(token, secretKey, JwsAlgorithm.HS256);
        }

        public string ForgotPasswordRequestToken(string email)
        {
            ForgotPasswordRequestTokenDto token = new ForgotPasswordRequestTokenDto(email);
            return JWT.Encode(token, secretKey, JwsAlgorithm.HS256);
        }

        public T ValidateDtoToken<T>(string tokenString)
        {
            T validatedDtoToken;
            try
            {
                validatedDtoToken = JWT.Decode<T>(tokenString, secretKey);
                var t = validatedDtoToken.GetType().GetProperties();
                long now = DateTimeHelper.GetUnixTimestamp();
                foreach (var item in validatedDtoToken.GetType().GetProperties())
                {
                    if (item.Name.ToLower().Equals("expiration"))
                    {
                        var expDate = validatedDtoToken.GetType().GetProperty(item.Name).GetValue(validatedDtoToken);
                        if ((long)expDate > now) throw new Exception("Token is expired");
                        Console.WriteLine(item);
                    }
                }
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
