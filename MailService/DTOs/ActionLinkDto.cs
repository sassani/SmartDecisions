using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailService.DTOs
{
    public class ActionLinkDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
    }
}
