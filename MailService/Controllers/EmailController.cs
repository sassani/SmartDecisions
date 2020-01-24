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
    public class EmailController : ControllerBase
    {
        private readonly IEmailServer mailServer;
        public EmailController(IEmailServer mailServer)
        {
            this.mailServer = mailServer;
        }

        [HttpPost]
        public void SendVerificationEmail([FromBody] EmailVerificationDto data)
        {
            mailServer.SendVerification(data.Email, data.Token);
        }
    }
}