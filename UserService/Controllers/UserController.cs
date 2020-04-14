using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shared.Controllers;
using Shared.Response;

namespace UserService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        // GET: api/User
        [HttpGet]
        public IActionResult Get()
        {
            string endPointCode = "01";

            var payload = new
            {
                controllerCode = ErrorCode,
            };
            return new Response(HttpStatusCode.Accepted, payload).ToActionResult();
        }
    }
}
