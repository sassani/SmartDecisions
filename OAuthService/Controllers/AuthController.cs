using Shared.Response;
using IdentityService.Core.Domain;
using IdentityService.Core.Domain.DTOs;
using IdentityService.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Filters;
using Shared.Attributes;

namespace IdentityService.Controllers
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
            this.clientService = clientService;
        }

        /// <summary>
        /// </summary>
        /// <error-code>01</error-code>
        /// <param name="login User Credential"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [EndPointData("01")]
        [HttpPost()]
        public async Task<IActionResult> Login([FromBody] CredentialDto crDto)
        {
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
                            Code = ErrorCode+"01",
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
                            Code = ErrorCode+"04",
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
                        Code = ErrorCode+"02",
                        Title = "Incorrect Credential",
                        Detail = "Refresh token is incorrect or expired."
                    } }).ToActionResult();
                    }
                    return new Response(HttpStatusCode.Forbidden,
                        new Error[] { new Error {
                        Code = ErrorCode+"03",
                        Title = "Incorrect Credential",
                        Detail = "Email or password is incorrect."
                    } }).ToActionResult();
                }
            }
            catch (System.Exception err)
            {

                return new Response(HttpStatusCode.Conflict,
                           new Error[] { new Error {
                            Code = ErrorCode+"05",
                            Title = "Login Error",
                            Detail = err.Message
                        } }).ToActionResult();
            }

        }


        [EndPointData("02")]
        [HttpDelete()]
        public IActionResult Logout()
        {
            if (credentialSrvice.Logout(GetLogsheetId()))
            {

                return new Response(HttpStatusCode.Accepted).ToActionResult();
            }

            return new Response(HttpStatusCode.BadGateway,
                        new Error[] { new Error {
                            Code = ErrorCode+"01",
                            Title = "Invalid data.",
                            Detail = "Maybe you have signed out before!"
                        } }).ToActionResult();
        }

        [EndPointData("03")]
        [HttpDelete("all")]
        public IActionResult LogoutAll()
        {
            if (credentialSrvice.Logout(GetLogsheetId(), true))
            {
                return new Response(HttpStatusCode.Accepted).ToActionResult();
            }

            return new Response(HttpStatusCode.BadGateway,
                        new Error[] { new Error {
                            Code = ErrorCode+"01",
                            Title = "Invalid data.",
                            Detail = "Maybe you have signed out before!"
                        } }).ToActionResult();
        }
    }
}