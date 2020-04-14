using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Response;
using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;
using OAuthService.Core.Services.Interfaces;
using Filters;
using Shared.Attributes;

namespace OAuthService.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ValidateModelAttributeFilter))]
    public class CredentialController : BaseController
    {
        public CredentialController(ICredentialService credentialSrvice) : base(credentialSrvice)
        {
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

        [AllowAnonymous]
        [EndPointData("01")]
        [HttpPost()]
        public async Task<IActionResult> AddCredential([FromBody] CredentialDto crDto)
        {
            try
            {
                if (await credentialSrvice.IsEmailExistedAsync(crDto.Email!))
                {
                    return new Response(HttpStatusCode.Conflict,
                            new Error[] { new Error {
                            Code = ErrorCode+"02",
                            Title = "Invalid Email",
                            Detail = "This email address is already being used."
                        } }).ToActionResult();
                }

                await credentialSrvice.RegisterAsync(crDto);
                return new Response(HttpStatusCode.OK, crDto.Email!).ToActionResult();
            }
            catch (Exception err)
            {

                return new Response(HttpStatusCode.Conflict,
                        new Error[] { new Error {
                            Code = ErrorCode+"03",
                            Title = "Registering Error",
                            Detail = err.Message
                        } }).ToActionResult();
            }
        }

        [AllowAnonymous]
        [EndPointData("02")]
        [HttpGet("emailverification/{email}")]
        public async Task<IActionResult> EmailVerificationRequest(string email)
        {
            try
            {
                var cr = await GetCredentialAsync();
                await credentialSrvice.SendEmailVerificationToken(email);
                return new Response(HttpStatusCode.OK, email).ToActionResult();
            }
            catch (Exception err)
            {
                return new Response(HttpStatusCode.BadRequest,
                        new Error[] { new Error {
                            Code =ErrorCode+"01",
                            Title = "Email Verification Request Hass Been Failed",
                            Detail = err.Message
                        } }).ToActionResult();
            }
        }

        [AllowAnonymous]
        [EndPointData("06")]
        [HttpPost("emailverification")]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailVerificationDto evDto)
        {
            try
            {
                await credentialSrvice.VerifyEmailAsync(evDto.Token);
                return new Response(HttpStatusCode.OK).ToActionResult();
            }
            catch (Exception err)
            {
                return new Response(HttpStatusCode.BadRequest,
                        new Error[] { new Error {
                            Code = ErrorCode+"01",
                            Title = "Email Verification Failed",
                            Detail = err.Message
                        } }).ToActionResult();
            }
        }

        [AllowAnonymous]
        [EndPointData("03")]
        [HttpGet("forgotpassword/{email}")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                // Check Email Address
                if (await credentialSrvice.IsEmailExistedAsync(email))
                {
                    await credentialSrvice.SendForgotPasswordRequestLinkAsync(email);
                    return new Response(HttpStatusCode.OK, email).ToActionResult();
                }
                else
                {
                    return new Response(HttpStatusCode.BadRequest,
                        new Error[] { new Error {
                            Code = ErrorCode+"01",
                            Title = "Invalid Email Address",
                            Detail = "The provided email address is not registered in our system"
                        } }).ToActionResult();
                }
            }
            catch (Exception err)
            {

                return new Response(HttpStatusCode.BadRequest,
                       new Error[] { new Error {
                            Code = ErrorCode+"02",
                            Title = "Password Change has been Failed",
                            Detail = err.Message
                        } }).ToActionResult();
            }
        }

        [AllowAnonymous]
        [EndPointData("04")]
        [HttpPut("forgotpassword")]
        public async Task<IActionResult> ForgotPasswordChange([FromBody] CredentialDto crDto)
        {
            try
            {
                Credential cr = await credentialSrvice.CreateCredentialAsync(crDto);
                return await ChangePassword(cr, crDto.NewPassword!);
            }
            catch (Exception err)
            {

                return new Response(HttpStatusCode.BadRequest,
                       new Error[] { new Error {
                            Code = ErrorCode+"02",
                            Title = "Password Change has been Failed",
                            Detail = err.Message
                        } }).ToActionResult();
            }
        }

        [EndPointData("05")]
        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword([FromBody] CredentialDto crDto)
        {
            try
            {
                Credential cr = await credentialSrvice.CreateCredentialAsync(crDto, GetCredentialId());
                if (!cr.IsAuthenticated)
                {
                    return new Response(HttpStatusCode.Forbidden,
                    new Error[] { new Error {
                            Code = ErrorCode+"01",
                            Title = "Old Password is wrong",
                            Detail = "Try forget password instead."
                        } }).ToActionResult();
                }
                return await ChangePassword(cr, crDto.NewPassword!);
            }
            catch (Exception err)
            {

                return new Response(HttpStatusCode.BadRequest,
                       new Error[] { new Error {
                            Code = ErrorCode+"02",
                            Title = "Password Change has been Failed",
                            Detail = err.Message
                        } }).ToActionResult();
            }
        }

        [EndPointData("06")]
        private async Task<IActionResult> ChangePassword(Credential cr, string newPassword)
        {
            try
            {
                if (!cr.IsEmailVerified)
                {
                    return new Response(HttpStatusCode.Forbidden,
                    new Error[] { new Error {
                            Code = ErrorCode+"01",
                            Title = "Not Verified Email Address",
                            Detail = "This email address was not verified. Please verify your email before changing password."
                        } }).ToActionResult();
                }
                await credentialSrvice.ChangePasswordAsync(cr, newPassword);

                return new Response(HttpStatusCode.OK, "password has been successfully changed").ToActionResult();
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
    }
}