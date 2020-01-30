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
        private readonly ICredentialService credentialService;
        private readonly IClientService clientService;

        public CredentialController(ICredentialService credentialSrvice, IClientService clientService, ICredentialService credentialService) : base(credentialSrvice)
        {
            ErrorCode = "02";
            this.credentialService = credentialService;
            this.clientService = clientService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> RegisterCredential([FromBody] CredentialDto credential)
        {
            string errCode = "01";
            try
            {
                Client client = await clientService.CreateClientAsync(credential.ClientId, credential.ClientSecret);

                if (!client.IsValid)
                {
                    return new Response(HttpStatusCode.Forbidden,
                            new Error[] { new Error {
                            Code = ErrorCode+errCode+"01",
                            Title = "Invalid Client",
                            Detail = "Client info is incorrect."
                        } }).ToActionResult();
                }

                // check history
                if (credentialService.IsEmailExisted(credential.Email))
                {
                    return new Response(HttpStatusCode.Conflict,
                            new Error[] { new Error {
                            Code = ErrorCode+errCode+"02",
                            Title = "Invalid Email",
                            Detail = "This email address is already being used."
                        } }).ToActionResult();
                }

                await credentialService.Register(credential);
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
                await credentialService.VerifyEmail(token);
                return new Response(HttpStatusCode.OK, null).ToActionResult();
            }
            catch (Exception err)
            {

                // TODO: add 2 types of responses
                // 1- if everything gos well --> redirect user to the web page or show somthing as OK
                return new Response(HttpStatusCode.NotModified,
                        new Error[] { new Error {
                            Code = ErrorCode+errCode+"01",
                            Title = "Email Verification Error",
                            Detail = err.Message
                        } }).ToActionResult();
            }
        }
    }
}