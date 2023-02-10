using System;
using System.Dynamic;
using System.Net;
using System.Threading.Tasks;
using ApplicationService.Core.DAL;
using MathEngine.AHP;
using Microsoft.AspNetCore.Mvc;
using Shared.Attributes;
using Shared.Response;
using Shared.Responses;

namespace ApplicationService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AhpController : BaseController
    {
        public AhpController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {}

        [EndPointData("01")]
        [HttpPost]
        public async Task<IActionResult> SolveModel([FromBody] ExpandoObject eoInput)
        {
            try
            {
                dynamic eo = eoInput;
                AHPModel ahpModel = new AHPModel();
                ahpModel.CreateAhpModelFromJsonString(eo.ahpModel.ToString());
                ahpModel.RunDFS();
                return new Response(HttpStatusCode.OK, ahpModel.ReportAsJson()).ToActionResult();
            }
            catch (Exception err)
            {
                return Errors.InternalServer(ErrorCode + "01", detail: err.Message);
            }

        }

    }
}
