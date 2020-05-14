using System.Collections.Generic;

namespace ApplicationService.Core.Domain
{
    public class Profile : Owner
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? ImageUrl { get; set; }
        public Avatar? Avatar { get; set; }
        public string? Company { get; set; }
        public virtual List<Address>? Addresses { get; set; }
    }
}
