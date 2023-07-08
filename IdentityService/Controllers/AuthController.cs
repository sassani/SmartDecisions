using System;
using System.Net;
using System.Threading.Tasks;
using Filters;
using IdentityService.Core.Domain;
using IdentityService.Core.Domain.DTOs;
using IdentityService.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Attributes;
using Shared.Response;
using Shared.Responses;
using static IdentityService.CONSTANTS;

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
        /// <param name="crDto">credential DTO</param>
        /// <returns></returns>
        [AllowAnonymous]
        [EndPointData("01")]
        [HttpPost()]
        public async Task<IActionResult> Login([FromBody] CredentialDto crDto)
        {
            try
            {
                Client client = new Client();
                if (crDto.RequestType.ToLower().Equals(REQUEST_TYPE.ID_TOKEN))
                {
                    client = await clientService.CreateClientAsync(crDto.ClientId!, crDto.ClientSecret);

                    if (!client.IsValid) return Errors.Forbiden(
                             ErrorCode + "01",
                             "Invalid Client",
                             "Client info is incorrect.");
                }

                Credential credential = await credentialSrvice.CreateCredentialAsync(crDto);
                if (credential.IsAuthenticated)
                {
                    // check user
                    if (!credential.IsActive) return Errors.Forbiden(
                             ErrorCode + "04",
                             "Unavalable User",
                             "Your account is suspended");

                    var payload = new
                    {
                        authToken = await credentialSrvice.LoginAsync(credential, client)
                    };
                    return new Response(HttpStatusCode.Accepted, payload).ToActionResult();
                }
                else
                {
                    if (crDto.RequestType.ToLower().Equals(REQUEST_TYPE.REFRESH_TOKEN)) return Errors.Forbiden(
                         ErrorCode + "02",
                         "Incorrect Credential",
                         "Refresh token is incorrect or expired.");

                    return Errors.Forbiden(
                        ErrorCode + "03",
                        "Incorrect Credential",
                        "Email or password is incorrect.");
                }
            }
            catch (Exception err)
            {
                string? details = null;
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() == "development")
                {
                    details = err.InnerException?.Message;
                }
                return Errors.InternalServer(ErrorCode + "05", "Login failed", details);
            }

        }


        [EndPointData("02")]
        [HttpDelete()]
        public async Task<IActionResult> Logout()
        {
            if (await credentialSrvice.LogoutAsync(GetLogsheetId()))
                return new Response(HttpStatusCode.Accepted).ToActionResult();

            return Errors.NotFound(ErrorCode + "01", "Invalid data.", "Maybe you have signed out before!");
        }

        [EndPointData("03")]
        [HttpDelete("all")]
        public async Task<IActionResult> LogoutAll()
        {
            if (await credentialSrvice.LogoutAsync(GetLogsheetId(), true))
                return new Response(HttpStatusCode.Accepted).ToActionResult();

            return Errors.NotFound(ErrorCode + "01", "Invalid data.", "Maybe you have signed out before!");
        }
    }
}