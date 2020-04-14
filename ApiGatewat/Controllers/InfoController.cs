using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Response;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetInfoSecure()
        {
            var payload = new
            {
                Message = "Smart Solutions Web Api v1."
            };
            return new Response(HttpStatusCode.Accepted, payload).ToActionResult();
        }
    }
}