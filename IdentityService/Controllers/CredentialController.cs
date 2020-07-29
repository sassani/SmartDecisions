using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Response;
using IdentityService.Core.Domain;
using IdentityService.Core.Domain.DTOs;
using IdentityService.Core.Services.Interfaces;
using Filters;
using Shared.Attributes;
using Shared.ErrorHandlers;
using IdentityService.Core.DAL;
using Shared.Responses;
using IntraServicesApi;

namespace IdentityService.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ValidateModelAttributeFilter))]
    public class CredentialController : BaseController
    {
        private IUnitOfWork unitOfWork;
        public CredentialController(ICredentialService credentialSrvice, IUnitOfWork unitOfWork) : base(credentialSrvice)
        {
            this.unitOfWork = unitOfWork;
        }

        [EndPointData("00")]
        [HttpGet]
        public async Task<IActionResult> GetCredentialInfo()
        {
            var cr = await GetCredentialAsync();
            var payload = new
            {
                cr.PublicId,
                cr.Email,
                cr.IsEmailVerified
            };
            return new Response(HttpStatusCode.OK, payload).ToActionResult();
        }

        #region Registration & Verification
        [AllowAnonymous]
        [EndPointData("01")]
        [HttpPost()]
        public async Task<IActionResult> AddCredential([FromBody] CredentialDto crDto)
        {
            try
            {
                await credentialSrvice.RegisterAsync(crDto);
                var payload = new
                {
                    registeredEmail = crDto.Email,
                    verificationEmailHasBeenSent = true
                };
                return new Response(HttpStatusCode.OK, payload).ToActionResult();
            }
            catch (IntraServiceException err)
            {
                var payload = new
                {
                    registeredEmail = crDto.Email,
                    verificationEmailHasBeenSent = false,
                    details = err.Message
                };
                return new Response(HttpStatusCode.OK, payload).ToActionResult();
            }
            catch (BaseException err)
            {
                return Errors.BaseExceptionResponse(err, ErrorCode + "01");
            }
            catch (Exception)
            {
                return Errors.InternalServer(ErrorCode + "00", "Registering Error");
            }
        }

        [AllowAnonymous]
        [EndPointData("02")]
        [HttpGet("emailverification/{email}")]
        public async Task<IActionResult> RequestEmailVerificationToken(string email)
        {
            try
            {
                await credentialSrvice.SendEmailVerificationTokenAsync(email);
                return new Response(HttpStatusCode.OK, new { message = $"We've just sent an email to ({email}). Please check your email" }).ToActionResult();
            }
            catch (BaseException err)
            {
                return Errors.BaseExceptionResponse(err, ErrorCode + "01");
            }
            catch (Exception)
            {
                return Errors.InternalServer(ErrorCode + "00", "Email Verification Request Hass Been Failed");
            }
        }

        [AllowAnonymous]
        [EndPointData("06")]
        [HttpPost("emailverification")]
        public async Task<IActionResult> VerifyEmailByToken([FromBody] EmailVerificationDto evDto)
        {
            try
            {
                await credentialSrvice.VerifyEmailAsync(evDto.Token);
                return new Response(HttpStatusCode.OK).ToActionResult();
            }
            catch (BaseException err)
            {
                return Errors.BaseExceptionResponse(err, ErrorCode + "01");
            }
            catch (Exception)
            {
                return Errors.InternalServer(ErrorCode + "00", "Email Verification Failed");
            }
        }
        #endregion

        #region Password
        [AllowAnonymous]
        [EndPointData("03")]
        [HttpGet("forgotpassword/{email}")]
        public async Task<IActionResult> RequestForgotPasswordToken(string email)
        {
            try
            {
                await credentialSrvice.SendForgotPasswordRequestLinkAsync(email);
                return new Response(HttpStatusCode.OK, new { message = $"We've just sent an email to ({email}). Please check your email" }).ToActionResult();

            }
            catch (BaseException err)
            {
                return Errors.BaseExceptionResponse(err, ErrorCode + "01");
            }
            catch (Exception)
            {
                return Errors.InternalServer(ErrorCode + "00", "Password Change has been Failed");
            }
        }

        [AllowAnonymous]
        [EndPointData("04")]
        [HttpPut("forgotpassword")]
        public async Task<IActionResult> ForgotPasswordChangeByToken([FromBody] CredentialDto crDto)
        {
            try
            {
                await credentialSrvice.ChangePasswordAsync(crDto);
                return new Response(HttpStatusCode.OK, new { message = "password has been successfully changed" }).ToActionResult();

            }
            catch (BaseException err)
            {
                return Errors.BaseExceptionResponse(err, ErrorCode + "01");
            }
            catch (Exception)
            {
                return Errors.InternalServer(ErrorCode + "00", "Password Change has been Failed");
            }
        }

        [EndPointData("05")]
        [HttpPut("password")]
        public async Task<IActionResult> ChangePasswordByOldPassword([FromBody] CredentialDto crDto)
        {
            try
            {
                await credentialSrvice.ChangePasswordAsync(crDto, GetCredentialId());
                return new Response(HttpStatusCode.OK, new { message = "password has been successfully changed" }).ToActionResult();
            }
            catch (BaseException err)
            {
                return Errors.BaseExceptionResponse(err, ErrorCode + "01");
            }
            catch (Exception)
            {
                return Errors.InternalServer(ErrorCode + "00", "Password Change has been Failed");
            }
        }
        #endregion
    }
}