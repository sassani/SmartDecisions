using RestApi.Controllers.Responses;
using RestApi.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace RestApi.Controllers
{
	[AllowAnonymous]
	[Route("api/info")]
	[ApiController]
	public class InfoController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        public InfoController(ILogger<WeatherForecastController> logger)
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