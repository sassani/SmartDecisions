using System;
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
        //private readonly IClientService clientService;

        public CredentialController(ICredentialService credentialSrvice) : base(credentialSrvice)
        {
            ErrorCode = "02";
            //this.clientService = clientService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> AddCredential([FromBody] CredentialDto credential)
        {
            string errCode = "01";
            try
            {
                if (credentialSrvice.IsEmailExisted(credential.Email))
                {
                    return new Response(HttpStatusCode.Conflict,
                            new Error[] { new Error {
                            Code = ErrorCode+errCode+"02",
                            Title = "Invalid Email",
                            Detail = "This email address is already being used."
                        } }).ToActionResult();
                }

                await credentialSrvice.Register(credential);
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
                await credentialSrvice.VerifyEmail(token);
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

        //[AllowAnonymous]
        //[HttpPost()]
    }
}