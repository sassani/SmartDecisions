using OAuthService.Core.Domain;
using Helpers;
using OAuthService.Core.Services.Interfaces;
using OAuthService.Core.DAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OAuthService.Core.Domain.DTOs;
using IntraServicesApi;
using Microsoft.CodeAnalysis.Options;
using OAuthService.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;

namespace OAuthService.Core.Services
{
    public class CredentialService : ICredentialService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITokenService tokenSrvice;
        private string refreshToken;
        private Logsheet logsheet;
        private Credential credential;
        private readonly IOptions<AppSettingsModel> config;

        public CredentialService(IUnitOfWork unitOfWork, ITokenService tokenSrvice, IOptions<AppSettingsModel> config)
        {
            this.unitOfWork = unitOfWork;
            this.tokenSrvice = tokenSrvice;
            credential = new Credential();
            logsheet = new Logsheet();
            refreshToken = default!;
            this.config = config;
        }

        public async Task<Credential> CreateCredentialAsync(CredentialDto crDto)
        {
            try
            {
                switch (crDto.RequestType.ToLower())
                {
                    case CONSTANTS.REQUEST_TYPE.REFRESH_TOKEN:
                        {
                            refreshToken = crDto.RefreshToken!;
                            logsheet = await unitOfWork.Logsheet.FindLogsheetByRefreshTokenAsync(refreshToken);
                            if (logsheet != null && logsheet.Credential != null)
                            {
                                credential = logsheet.Credential;
                                credential.IsAuthenticated = true;
                            }
                            break;
                        }
                    case CONSTANTS.REQUEST_TYPE.ID_TOKEN:
                        {
                            var credentialDb = await unitOfWork.Credential.FindByEmailAsync(crDto.Email!);
                            if (credentialDb != null)
                            {
                                credential = credentialDb;
                                credential.IsAuthenticated = CheckPassword(credential, crDto);
                            }
                            break;
                        }
                    case CONSTANTS.REQUEST_TYPE.FORGOT_PASSWORD:
                        {
                            ForgotPasswordRequestTokenDto validatedToken = tokenSrvice.ValidateDtoToken<ForgotPasswordRequestTokenDto>(crDto.ResetPasswordToken!);
                            var credentialDb = await unitOfWork.Credential.FindByEmailAsync(validatedToken.Email);
                            if (credentialDb != null) credential = credentialDb;
                            break;
                        }
                    default:
                        break;
                }
                return credential;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<Credential> CreateCredentialAsync(CredentialDto crDto, string uid)
        {
            try
            {
                var credentialDb = await unitOfWork.Credential.FindByUidAsync(uid);
                if (credentialDb != null)
                {
                    credential = credentialDb;
                    credential.IsAuthenticated = CheckPassword(credential, crDto);
                }
                return credential;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<Credential> CreateCredentialAsync(string uid)
        {
            try
            {
                var credentialDb = await unitOfWork.Credential.FindByUidAsync(uid);
                if (credentialDb != null)
                {
                    credential = credentialDb;
                }
                return credential;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        private bool CheckPassword(Credential cr, CredentialDto crDto)
        {
            return StringHelper.CompareStringToHash(cr.Password, crDto.Password);
        }

        public AuthTokenDto Login(Credential credential, Client? client = null)
        {
            if (refreshToken == null && client != null)
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
                unitOfWork.Logsheet.UpdateLastTimeLogin(logsheet);
            }
            unitOfWork.Credential.UpdateLastLogin(credential);
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

        public async Task RegisterAsync(CredentialDto credential)
        {
            try
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

                //string url = config.Value.RedirectUrls.EmailVerification;
                //MailServiceApi ms = new MailServiceApi(config.Value.ServicesApiKeys.MailService);
                //await ms.SendVerificationEmail(credential.Email, $"{url}/{tokenSrvice.EmailVerificationToken(credential.Email)}");
                await SendEmailVerificationToken(credential.Email);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task SendEmailVerificationToken(string email)
        {
            if (await IsEmailExistedAsync(email)) {
            string url = config.Value.RedirectUrls.EmailVerification;
            MailServiceApi ms = new MailServiceApi(config.Value.ServicesApiKeys.MailService);
            await ms.SendVerificationEmail(email, $"{url}/{tokenSrvice.EmailVerificationToken(email)}");
            }
            else
            {
                throw new Exception("This Email is not registered in our system");
            }
        }

        public async Task<bool> IsEmailExistedAsync(string email) =>  await unitOfWork.Credential.IsEmailExistAsync(email);

        public async Task VerifyEmailAsync(string token)
        {
            try
            {
                var validatedToken = tokenSrvice.ValidateDtoToken<EmailVerificationTokenDto>(token);
                await unitOfWork.Credential.VerifyEmailAsync(validatedToken.Email);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task SendForgotPasswordRequestLinkAsync(string email)
        {
            string uri = config.Value.RedirectUrls.ForgotPasswordChange;
            try
            {
                MailServiceApi ms = new MailServiceApi(config.Value.ServicesApiKeys.MailService);
                await ms.SendForgotPasswordLink(email, $"{uri}/{tokenSrvice.ForgotPasswordRequestToken(email)}");
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

        }

        public async Task ChangePasswordAsync(Credential cr, string newPass)
        {
            try
            {
                cr.Password = StringHelper.StringToHash(newPass);
                await Task.Run(() => unitOfWork.Complete());
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

    }
}
