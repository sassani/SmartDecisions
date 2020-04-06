using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Core.Domain
{
    public class User
    {
        public User() => Addresss = new HashSet<Address>();

        public int Id { get; set; }
        public string PublicId { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? ImageUrl { get; set; }
        public string? Company { get; set; }
        public string? PhoneNumber { get; set; }
        public virtual ICollection<Address> Addresss { get; set; }
    }
}
