using System;
using System.Threading.Tasks;
using MailService.DTOs;
using MailService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MailService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActionLinkController : ControllerBase
    {
        private readonly IEmailServer mailServer;
        public ActionLinkController(IEmailServer mailServer)
        {
            this.mailServer = mailServer;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] ActionLinkDto data)
        {
            try
            {
                await mailServer.SendActionLink(data);
                return Ok(data.Email);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}