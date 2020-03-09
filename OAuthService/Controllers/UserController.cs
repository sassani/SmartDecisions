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
    }
}