using System.Collections.Generic;

namespace DecissionService.Core.Domain
{
    public class User
    {
        //public User() => Addresses = new List<Address>();

        public string Id { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? ImageUrl { get; set; }
        public string? Company { get; set; }
        public virtual List<Address>? Addresses { get; set; }
    }
}
