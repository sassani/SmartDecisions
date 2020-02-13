﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OAuthService.Controllers.Responses;
using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;
using OAuthService.Core.Services.Interfaces;

namespace OAuthService.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CredentialController : BaseController
    {
        public CredentialController(ICredentialService credentialSrvice) : base(credentialSrvice)
        {
            ErrorCode = "02";
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> AddCredential([FromBody] CredentialDto credential)
        {
            string errCode = "01";
            try
            {
                if (await credentialSrvice.IsEmailExistedAsync(credential.Email))
                {
                    return new Response(HttpStatusCode.Conflict,
                            new Error[] { new Error {
                            Code = ErrorCode+errCode+"02",
                            Title = "Invalid Email",
                            Detail = "This email address is already being used."
                        } }).ToActionResult();
                }

                await credentialSrvice.RegisterAsync(credential);
                return new Response(HttpStatusCode.OK, credential.Email).ToActionResult();
            }
            catch (Exception err)
            {

                return new Response(HttpStatusCode.Conflict,
                        new Error[] { new Error {
                            Code = ErrorCode+errCode+"03",
                            Title = "Registering Error",
                            Detail = err.Message
                        } }).ToActionResult();
            }


        }

        [AllowAnonymous]
        [HttpGet("emailverification")]
        public async Task<IActionResult> VerifyEmail()
        {
            string errCode = "02";
            try
            {
                string token = Request.Query["evtoken"];
                await credentialSrvice.VerifyEmailAsync(token);
                return new Response(HttpStatusCode.OK, null).ToActionResult();
            }
            catch (Exception err)
            {
                return new Response(HttpStatusCode.Unauthorized,
                        new Error[] { new Error {
                            Code = ErrorCode+errCode+"01",
                            Title = "Email Verification Failed",
                            Detail = err.Message
                        } }).ToActionResult();
            }
        }

        [AllowAnonymous]
        [HttpGet("forgotpassword/{email}")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            string errCode = "03";
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
                    return new Response(HttpStatusCode.Unauthorized,
                        new Error[] { new Error {
                            Code = ErrorCode+errCode+"01",
                            Title = "Invalid Email Address",
                            Detail = "The provided email address is not registered in our system"
                        } }).ToActionResult();
                }
            }
            catch (Exception err)
            {

                return new Response(HttpStatusCode.Unauthorized,
                       new Error[] { new Error {
                            Code = ErrorCode+errCode+"02",
                            Title = "Password Change has been Failed",
                            Detail = err.Message
                        } }).ToActionResult();
            }
        }

        [AllowAnonymous]
        [HttpPut("forgotpassword")]
        public async Task<IActionResult> ForgotPasswordChange([FromBody] CredentialDto credential)
        {
            string errCode = "04";
            //if (credential is null) throw new ArgumentNullException(nameof(credential));
            try
            {
                Credential cr = await credentialSrvice.CreateCredentialAsync(credential);
                return await ChangePassword(cr, credential.NewPassword);
            }
            catch (Exception err)
            {

                return new Response(HttpStatusCode.Unauthorized,
                       new Error[] { new Error {
                            Code = ErrorCode+errCode+"02",
                            Title = "Password Change has been Failed",
                            Detail = err.Message
                        } }).ToActionResult();
            }
        }

        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword([FromBody] CredentialDto credential)
        {
            string errCode = "05";
            //if (credential is null) throw new ArgumentNullException(nameof(credential));

            try
            {
                Credential cr = await credentialSrvice.CreateCredentialAsync(credential, GetCredentialId());
                if (!cr.IsAuthenticated)
                {
                    return new Response(HttpStatusCode.Unauthorized,
                    new Error[] { new Error {
                            Code = ErrorCode+errCode+"01",
                            Title = "Old Password is wrong",
                            Detail = "Try forget password instead."
                        } }).ToActionResult();
                }
                return await ChangePassword(cr, credential.NewPassword);
            }
            catch (Exception err)
            {

                return new Response(HttpStatusCode.Unauthorized,
                       new Error[] { new Error {
                            Code = ErrorCode+errCode+"02",
                            Title = "Password Change has been Failed",
                            Detail = err.Message
                        } }).ToActionResult();
            }
        }

        private async Task<IActionResult> ChangePassword(Credential cr, string newPassword)
        {
            string errCode = "06";
            try
            {
                if (!cr.IsEmailVerified)
                {
                    return new Response(HttpStatusCode.Unauthorized,
                    new Error[] { new Error {
                            Code = ErrorCode+errCode+"01",
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