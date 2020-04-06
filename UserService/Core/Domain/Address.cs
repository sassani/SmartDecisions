using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Core.Domain
{
    public class Address
    {
        public Address() => User = new User();

        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Address1 { get; set; } = default!;
        public string? Address2 { get; set; }
        public string City { get; set; } = default!;
        public string? State { get; set; }
        public string Country { get; set; } = default!;
        public string? ZipCode { get; set; }
    }
}
