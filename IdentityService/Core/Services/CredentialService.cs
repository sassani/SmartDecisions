using IdentityService.Core.Domain;
using Helpers;
using IdentityService.Core.Services.Interfaces;
using IdentityService.Core.DAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Core.Domain.DTOs;
using IntraServicesApi;
using Microsoft.CodeAnalysis.Options;
using IdentityService.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Shared.ErrorHandlers;
using System.Net;
using static IdentityService.CONSTANTS;

namespace IdentityService.Core.Services
{
    public class CredentialService : ICredentialService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITokenService tokenSrvice;
        private string refreshToken;
        private Logsheet logsheet;
        private Credential credential;
        private readonly AppSettingsModel config;

        public CredentialService(IUnitOfWork unitOfWork, ITokenService tokenSrvice, IOptions<AppSettingsModel> options)
        {
            this.unitOfWork = unitOfWork;
            this.tokenSrvice = tokenSrvice;
            credential = new Credential();
            logsheet = new Logsheet();
            refreshToken = default!;
            config = options.Value;
        }

        #region Create Credential
        public async Task<Credential> CreateCredentialAsync(CredentialDto crDto)
        {
            try
            {
                switch (crDto.RequestType.ToLower())
                {
                    case REQUEST_TYPE.REFRESH_TOKEN:
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
                    case REQUEST_TYPE.ID_TOKEN:
                        {
                            var credentialDb = await unitOfWork.Credential.FindByEmailAsync(crDto.Email!);
                            if (credentialDb != null)
                            {
                                credential = credentialDb;
                                credential.IsAuthenticated = CheckPassword(credential, crDto);
                            }
                            break;
                        }
                    case REQUEST_TYPE.FORGOT_PASSWORD:
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
        #endregion

        #region Login-Logout
        public async Task<AuthTokenDto> LoginAsync(Credential credential, Client? client = null)
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
            await unitOfWork.CompleteAsync();
            return tokenSrvice.GenerateAuthToken(credential, logsheet.Id, refreshToken);
        }

        public async Task<bool> LogoutAsync(int LogintId, bool all = false)
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
            await unitOfWork.CompleteAsync();
            return true;
        }
        #endregion

        #region Registeration & Verification
        public async Task RegisterAsync(CredentialDto credential)
        {
            try
            {
                var cr = await unitOfWork.Credential.FindByEmailAsync(credential.Email!);
                if (cr != null) throw new BaseException(HttpStatusCode.Conflict, "Invalid Email", "This email address is already being used.");
                var newCredential = new Credential
                {
                    Email = credential.Email!,
                    Password = StringHelper.StringToHash(credential.Password),
                    IsActive = true,
                    PublicId = Guid.NewGuid().ToString(),
                };
                unitOfWork.Credential.Add(newCredential);
                await unitOfWork.CompleteAsync();

                //await SendEmailVerificationTokenAsync(credential.Email!);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public async Task SendEmailVerificationTokenAsync(string email)
        {
            var cr = await unitOfWork.Credential.FindByEmailAsync(email);
            if (cr == null) throw new BaseException(HttpStatusCode.NotFound, "Invalid email", "This Email is not registered in our system");
            if (cr.IsEmailVerified) throw new BaseException(HttpStatusCode.Conflict, "Verified Email", "This Email has been verified before");

            //try
            //{
                string url = config.RedirectUrls.EmailVerification;
                MailServiceApi ms = new MailServiceApi(config.SharedApiKey, config.EmailServerUrl);
                await ms.SendVerificationEmail(email, $"{url}/{tokenSrvice.EmailVerificationToken(email)}");
            //}
            //catch (IntraServiceException err)
            //{
            //    //throw new BaseException(HttpStatusCode.InternalServerError, err.Title, err.Description);

            //}

        }

        public async Task<bool> IsEmailExistedAsync(string email) => await unitOfWork.Credential.IsEmailExistAsync(email);

        public async Task VerifyEmailAsync(string token)
        {
            try
            {
                var validatedToken = tokenSrvice.ValidateDtoToken<EmailVerificationTokenDto>(token);
                var cr = await unitOfWork.Credential.FindByEmailAsync(validatedToken.Email);
                cr.IsEmailVerified = true;
                await unitOfWork.CompleteAsync();
            }
            catch (TokenException err)
            {
                throw new BaseException(err.Status, err.Title, err.Description);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        #endregion

        #region Password
        public async Task SendForgotPasswordRequestLinkAsync(string email)
        {
            try
            {
                //string uri = config.RedirectUrls.ForgotPasswordChange;
                var cr = await unitOfWork.Credential.FindByEmailAsync(email);
                if (cr == null) throw new BaseException(HttpStatusCode.NotFound, "Invalid Email Address", "The provided email address is not registered in our system");
                if (!cr.IsEmailVerified) throw new BaseException(HttpStatusCode.NotFound, "Not Verified Email Address", "This email address was not verified. Please verify your email before changing password.");

                MailServiceApi ms = new MailServiceApi(config.SharedApiKey, config.EmailServerUrl);
                await ms.SendForgotPasswordLink(email, $"{config.RedirectUrls.ForgotPasswordChange}/{tokenSrvice.ForgotPasswordRequestToken(email)}");
            }
            catch (IntraServiceException err)
            {
                throw new BaseException(err.Status, err.Title, err.Description);
            }

        }

        public async Task ChangePasswordAsync(CredentialDto crdto, string? uid = null)
        {
            try
            {
                if (uid != null)
                {
                    await CreateCredentialAsync(crdto, uid);
                    if (!CheckPassword(credential, crdto)) throw new BaseException(HttpStatusCode.Forbidden, "Wrong Password", "The old password is not matched. Try forgot password instead.");
                }
                else
                {
                    await CreateCredentialAsync(crdto);
                }

                credential.Password = StringHelper.StringToHash(crdto.NewPassword);
                await unitOfWork.CompleteAsync();
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
        #endregion
    }
}
