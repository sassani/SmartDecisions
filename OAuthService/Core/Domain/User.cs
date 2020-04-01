using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthService.Core.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string PublicId { get; set; } = default!;
        public virtual Credential Credential { get; set; } = new Credential();
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? ImageUrl { get; set; }
        public string? Company { get; set; }
        public string? PhoneNumber { get; set; }
        public Address? Address { get; set; }
    }
}
