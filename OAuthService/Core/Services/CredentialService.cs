using OAuthService.Core.Domain;
using OAuthService.Helpers;
using OAuthService.Core.Services.Interfaces;
using OAuthService.Core.DataServices;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OAuthService.Core.Domain.DTOs;

namespace OAuthService.Core.Services
{
    public class CredentialService : ICredentialService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITokenService tokenSrvice;
        private string refreshToken;
        private Logsheet logsheet;
        private Credential credential;

        public CredentialService(IUnitOfWork unitOfWork, ITokenService tokenSrvice)
        {
            this.unitOfWork = unitOfWork;
            this.tokenSrvice = tokenSrvice;
            credential = new Credential();
        }

        public async Task<Credential> CreateCredential(CredentialDto loginCredential)
        {
            Credential credentialDb;
            if (loginCredential.GrantType.ToLower().Equals("refreshtoken"))
            {
                refreshToken = loginCredential.RefreshToken;
                logsheet = await unitOfWork.Logsheet.FindLogsheetByRefreshTokenAsync(refreshToken);
                if (logsheet != null && logsheet.Credential != null)
                {
                    credential = logsheet.Credential;
                    credential.IsAuthenticated = true;
                }
            }
            else if (loginCredential.GrantType.ToLower().Equals("idtoken"))
            {
                credentialDb = unitOfWork.Credential.FindByEmail(loginCredential.Email);
                if (credentialDb != null && StringHelper.CompareStringToHash(credentialDb.Password, loginCredential.Password))
                {
                    credential = credentialDb;
                    credential.IsAuthenticated = true;
                }
            }
            return credential;
        }

        public AuthTokenDto Login(Client client, Credential credential)
        {
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
            else
            {

            }
            unitOfWork.Credential.UpdateLastLogin(credential);
            unitOfWork.Logsheet.UpdateLastTimeLogin(logsheet);
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

        public void Register(CredentialDto credential)
        {
            var newCredential = new Credential
            {
                Email = credential.Email,
                Password = StringHelper.StringToHash(credential.Password),
                IsActive = true,
                PublicId = Guid.NewGuid().ToString(),
            };
            unitOfWork.Credential.Add(newCredential);
            unitOfWork.Complete();
        }

        public Credential Get(int userId)
        {
            Credential user = new Credential();
            //UserDb userDb = unitOfWork.User.Get(userId);
            //Mapper.UserMapper(user, userDb);
            return user;
        }

        public Credential Get(string uuid)
        {
            Credential user = new Credential();
            //UserDb userDb = unitOfWork.User.Get(userId);
            //Mapper.UserMapper(user, userDb);
            return user;
        }

        public bool IsEmailExisted(string email)
        {
            return unitOfWork.Credential.IsEmailExist(email);
        }

        public void AddUserByUserInfo(RegisterUserDto user)
        {
            //UserDb newUser = new UserDb
            //{
            //	FirstName = user.FirstName,
            //	LastName = user.LastName,
            //	Email = user.Email,
            //	Password = StringHelper.StringToHash(user.Password)
            //};
            //unitOfWork.User.Add(newUser);
            //unitOfWork.Complete();
        }
    }
}
