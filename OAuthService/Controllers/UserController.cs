using OAuthService.Controllers.Responses;
using OAuthService.Core.Domain.DTOs;
using OAuthService.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace OAuthService.Controllers
{
    /// <error-code>02</error-code>
	[Authorize]
    [Route("user")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IClientService clientService;
        public UserController(ICredentialService userSrvice, IClientService clientService) : base(userSrvice)
        {
            ErrorCode = "02";
            this.clientService = clientService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto user)
        {
            string errCode = "01";
            // check email
            if (!await credentialSrvice.IsEmailExistedAsync(user.Email))
            {
                return new Response(HttpStatusCode.Conflict,
                            new Error[] { new Error {
                            Code = ErrorCode+errCode+"01",
                            Detail = "This email has already been taken."
                        } }).ToActionResult();
            }

            credentialSrvice.AddUserByUserInfo(user);

            var payload = new
            {
                email = user.Email,
                password = user.Password
            };
            return new Response(HttpStatusCode.Accepted, payload).ToActionResult();
        }
    }
}