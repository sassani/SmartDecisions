using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailService.DTOs
{
    public class EmailVerificationDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
