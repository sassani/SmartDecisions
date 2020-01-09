using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;
using OAuthService.Helpers;
using OAuthService.Core.Services.Interfaces;
using OAuthService.Core.DataServices;
using System;

namespace OAuthService.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITokenService tokenSrvice;
        private string refreshToken;

        public AuthService(IUnitOfWork unitOfWork, ITokenService tokenSrvice)
        {
            this.unitOfWork = unitOfWork;
            this.tokenSrvice = tokenSrvice;
        }

        public bool Authenticate(LoginCredentialDto loginCredential, ref Credential credential)
        {
            Credential userDb;
            if (loginCredential.GrantType.ToLower().Equals("refreshtoken"))
            {
                var dbUserClient = unitOfWork.Logsheet.FindByRefreshToken(loginCredential.RefreshToken);
                if (dbUserClient != null)
                {
                    //userDb = dbUserClient.User;
                    //userDb.Roles = unitOfWork.User.GetRoles(userDb);
                    //Mapper.UserMapper(user, userDb);
                    //refreshToken = loginUser.RefreshToken;
                    //userClientId = dbUserClient.Id;
                    return true;
                }
            }
            else if (loginCredential.GrantType.ToLower().Equals("idtoken"))
            {
                userDb = unitOfWork.Credential.FindByEmail(loginCredential.Email);
                if (userDb != null)
                {
                    if (StringHelper.CompareStringToHash(userDb.Password, loginCredential.Password))
                    {
                        //Mapper.UserMapper(user, userDb);
                        credential = userDb;
                        return true;
                    }
                }
            }
            return false;
        }

        public AuthTokenDto Login(Client client, Credential credential)
        {
            Logsheet logsheet = new Logsheet();
            if (refreshToken == null)
            {
                refreshToken = tokenSrvice.GenerateRefreshToken(credential.PublicId);
                logsheet = new Logsheet()
                {
                    RefreshToken = refreshToken,
                    CredentialId = credential.Id,
                    ClientId = client.Id,
                    Platform = client.Platform,
                    Browser = client.Browser,
                    IP = client.IP,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                unitOfWork.Logsheet.Add(logsheet);
            }
            unitOfWork.Credential.UpdateLastLogin(credential.Id);
            unitOfWork.Complete();
            return tokenSrvice.GenerateAuthToken(credential, logsheet.Id, refreshToken);
        }

        public bool Logout(int LogintId, bool all = false)
        {
            var ucdb = unitOfWork.Logsheet.Get(LogintId);
            if (ucdb == null) return false;
            if (all)
            {
                var ucdbs = unitOfWork.Logsheet.Find(uc => uc.ClientId == ucdb.ClientId && uc.CredentialId == ucdb.CredentialId);
                unitOfWork.Logsheet.RemoveRange(ucdbs);
            }
            else
            {
                unitOfWork.Logsheet.Remove(ucdb);
            }
            unitOfWork.Complete();
            return true;
        }
    }
}




