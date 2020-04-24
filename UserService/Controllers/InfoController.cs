using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.Attributes;
using Shared.Response;
using DecissionCore.Core.DAL;

namespace DecissionCore.Controllers
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
                    errorCode = ErrorCode + "00"
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

        [EndPointData("01")]
        [HttpGet("1")]
        public IActionResult GetInfoSecure()
        {
            try
            {
                var payload = new
                {
                    errorCode = ErrorCode + "00"
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