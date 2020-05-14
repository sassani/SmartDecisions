using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.Attributes;
using Shared.Response;
using ApplicationService.Core.DAL;

namespace ApplicationService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InfoController : BaseController
    {
        public InfoController(IUnitOfWork unitOfWork): base(unitOfWork)
        {
        }

        [EndPointData("00")]
        [HttpGet]
        public async Task<IActionResult> GetInfoSecureAsync()
        {
            try
            {
                var payload = new
                {
                    EndPointNumber = ErrorCode,
                    Message = "Smart Solutions Core is running..."
                };
                return new Response(HttpStatusCode.Accepted, payload).ToActionResult();

            }
            catch (Exception err)
            {
                return new Response(HttpStatusCode.Forbidden,
                                                new Error[] { new Error {
                            Code = ErrorCode+"01",
                            Title = "Invalid Client",
                            Detail = err.Message
                                            }}).ToActionResult();
            }
        }
    }
}