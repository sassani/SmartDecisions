using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailService.DTOs;
using MailService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
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
        public void Send([FromBody] ActionLinkDto data)
        {
            mailServer.SendActionLink(data);
        }
    }
}