using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shared.Response;

namespace ApiGateway.Controllers
{
    [Route("[controller]")]
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

        [HttpDelete]
        public IActionResult GetInfoSecureDel()
        {
            var payload = new
            {
                Message = "Smart Solutions Web Api v1."
            };
            return new Response(HttpStatusCode.Accepted, payload).ToActionResult();
        }
    }
}