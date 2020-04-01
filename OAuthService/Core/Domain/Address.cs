using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthService.Core.Domain
{
    public class Address
    {
        public string Address1 { get; set; } = default!;
        public string? Address2 { get; set; }
        public string City { get; set; } = default!;
        public string? State { get; set; }
        public string Country { get; set; } = default!;
        public string? ZipCode { get; set; }
    }
}
