﻿using OAuthService.Controllers.Responses;
using OAuthService.Core.Domain;
using OAuthService.Core.Domain.DTOs;
using OAuthService.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using OAuthService.Filters;

namespace OAuthService.Controllers
{
    /// <summary>
    /// </summary>
    /// <error-code>01</error-code>
    [Authorize]
    [ApiController]
    [ServiceFilter(typeof(ValidateModelAttributeFilter))]
    [Route("[controller]")]
    public class AuthController : BaseController
    {
        private readonly IClientService clientService;

        public AuthController(ICredentialService credentialSrvice, IClientService clientService) : base(credentialSrvice)
        {
            ErrorCode = "01";
            this.clientService = clientService;
        }

        /// <summary>
        /// </summary>
        /// <error-code>01</error-code>
        /// <param name="login User Credential"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> Login([FromBody] CredentialDto crDto)
        {
            string errCode = "01";

            //var validator = new CredentialDtoValidator();
            //var result = validator.Validate(loginCredential);
            //var errors = result.Errors;
            //if (!this.ModelState.IsValid)
            //{
            //    var t = "failed";
            //}
            try
            {
                Client client = new Client();
                if (crDto.RequestType.ToLower().Equals("idtoken"))
                {
                    client = await clientService.CreateClientAsync(crDto.ClientId!, crDto.ClientSecret);

                    if (!client.IsValid)
                    {
                        return new Response(HttpStatusCode.Forbidden,
                                new Error[] { new Error {
                            Code = ErrorCode+errCode+"01",
                            Title = "Invalid Client",
                            Detail = "Client info is incorrect."
                        } }).ToActionResult();
                    }
                }

                Credential credential = await credentialSrvice.CreateCredentialAsync(crDto);
                if (credential.IsAuthenticated)
                {
                    // check user
                    if (!credential.IsActive)
                    {
                        return new Response(HttpStatusCode.Forbidden,
                            new Error[] { new Error {
                            Code = ErrorCode+errCode+"04",
                            Title = "Unavalable User",
                            Detail = "Your account is suspended"
                        } }).ToActionResult();
                    }

                    var payload = new
                    {
                        authToken = credentialSrvice.Login(credential, client)
                    };
                    return new Response(HttpStatusCode.Accepted, payload).ToActionResult();
                }
                else
                {
                    if (crDto.RequestType.ToLower().Equals("refreshtoken"))
                    {
                        return new Response(HttpStatusCode.Forbidden,
                        new Error[] { new Error {
                        Code = ErrorCode+errCode+"02",
                        Title = "Incorrect Credential",
                        Detail = "Refresh token is incorrect or expired."
                    } }).ToActionResult();
                    }
                    return new Response(HttpStatusCode.Forbidden,
                        new Error[] { new Error {
                        Code = ErrorCode+errCode+"03",
                        Title = "Incorrect Credential",
                        Detail = "Email or password is incorrect."
                    } }).ToActionResult();
                }
            }
            catch (System.Exception err)
            {

                return new Response(HttpStatusCode.Conflict,
                           new Error[] { new Error {
                            Code = ErrorCode+errCode+"05",
                            Title = "Login Error",
                            Detail = err.Message
                        } }).ToActionResult();
            }

        }

        [HttpDelete()]
        public IActionResult Logout()
        {
            string errCode = "02";
            if (credentialSrvice.Logout(GetLogsheetId()))
            {

                return new Response(HttpStatusCode.Accepted).ToActionResult();
            }

            return new Response(HttpStatusCode.BadGateway,
                        new Error[] { new Error {
                            Code = ErrorCode+errCode+"01",
                            Title = "Invalid data.",
                            Detail = "Maybe you have signed out before!"
                        } }).ToActionResult();
        }

        [HttpDelete("all")]
        public IActionResult LogoutAll()
        {
            string errCode = "03";
            if (credentialSrvice.Logout(GetLogsheetId(), true))
            {
                return new Response(HttpStatusCode.Accepted).ToActionResult();
            }

            return new Response(HttpStatusCode.BadGateway,
                        new Error[] { new Error {
                            Code = ErrorCode+errCode+"01",
                            Title = "Invalid data.",
                            Detail = "Maybe you have signed out before!"
                        } }).ToActionResult();
        }
    }
}