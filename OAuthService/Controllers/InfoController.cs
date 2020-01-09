using OAuthService.Controllers.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace OAuthService.Controllers
{
	[AllowAnonymous]
	[Route("api/info")]
	[ApiController]
	public class InfoController : ControllerBase
    {
        private readonly ILogger<InfoController> _logger;
        public InfoController(ILogger<InfoController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
		public IActionResult GetInfo()
		{
			var payload = new
			{
				Data = "This is initial data"
			};
			return new Response(HttpStatusCode.Accepted, payload).ToActionResult();
		}
	}
}