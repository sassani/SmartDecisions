﻿using OAuthService.Controllers.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.IO;

namespace OAuthService.Controllers
{
    [Authorize]
    [Route("info")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly ILogger<InfoController> _logger;
        public InfoController(ILogger<InfoController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet()]
        public IActionResult GetInfo()
        {
            var payload = new
            {
                Message = "This is initial data"
            };
            return new Response(HttpStatusCode.Accepted, payload).ToActionResult();
        }

        [HttpGet("secure")]
        public IActionResult GetInfoSecure()
        {
            var payload = new
            {
                Message = "This is initial secure data"
            };
            return new Response(HttpStatusCode.Accepted, payload).ToActionResult();
        }
    }
}