using System;
using System.Linq;
using Helpers;
using Jose;
using Microsoft.Extensions.Options;
using IdentityService.Core.Domain;
using IdentityService.Core.Domain.DTOs;
using IdentityService.Core.Services.Interfaces;
using IdentityService.Extensions;
using Shared.ErrorHandlers;
using System.Net;

namespace IdentityService.Core.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppSettingsModel config;
        private readonly byte[] secretKey;
        private readonly JwsAlgorithm jwsAlg;
        public TokenService(IOptions<AppSettingsModel> options)
        {
            config = options.Value;
            secretKey = options.Value.Token.SecretKey.Select(x => (byte)x).ToArray();
            jwsAlg = JwsAlgorithm.HS256;
        }

        public AuthTokenDto GenerateAuthToken(Credential credential, int userClientId, string refreshToken)
        {
            AccessTokenDto accessToken = new AccessTokenDto(credential, userClientId, config.Token.ValidationPeriod);
            var accessTokenJson = System.Text.Json.JsonSerializer.Serialize(accessToken);
            string signedAccessToken = JWT.Encode(accessTokenJson, secretKey, jwsAlg);
            return new AuthTokenDto(signedAccessToken, refreshToken, "bearer", credential);
        }

        public string EmailVerificationToken(string email)
        {
            EmailVerificationTokenDto token = new EmailVerificationTokenDto(email);
            return JWT.Encode(token, secretKey, jwsAlg);
        }

        public string ForgotPasswordRequestToken(string email)
        {
            ForgotPasswordRequestTokenDto token = new ForgotPasswordRequestTokenDto(email);
            return JWT.Encode(token, secretKey, jwsAlg);
        }

        public string GenerateRefreshToken(string userPublicId)
        {
            return userPublicId + StringHelper.GenerateRandom(37);
        }

        public T ValidateDtoToken<T>(string tokenString)
        {
            if (tokenString.Split('.').Length != 3) throw new TokenException(HttpStatusCode.BadRequest, "Invalid Token", "Token must have three segments.");
            T validatedDtoToken;
            try
            {
                validatedDtoToken = JWT.Decode<T>(tokenString, secretKey,jwsAlg);
                if (validatedDtoToken == null) throw new TokenException(HttpStatusCode.BadRequest, "Invalid Token", "Signature validation failed.");
                long now = DateTimeHelper.GetUnixTimestamp();
                foreach (var item in validatedDtoToken.GetType().GetProperties())
                {
                    if (item.Name.ToLower().Equals("expiration"))
                    {
                        var expDate = validatedDtoToken.GetType().GetProperty(item.Name)!.GetValue(validatedDtoToken);
                        if ((long)expDate! < now) throw new TokenException(HttpStatusCode.BadRequest, "Invalid Token", "The token is expired.");
                    }
                }
            }
            catch (ArgumentOutOfRangeException err)
            {
                throw new TokenException(HttpStatusCode.BadRequest, "Invalid token", err.Message);
            }
            catch (IntegrityException err)
            {
                throw new TokenException(HttpStatusCode.BadRequest, "Invalid token", err.Message);
            }
            return validatedDtoToken;
        }

        
    }
}
