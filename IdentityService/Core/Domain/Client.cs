using IdentityService.Core.DAL;
using Helpers;
using Microsoft.AspNetCore.Http;
using UAParser;
using System.Collections.Generic;

namespace IdentityService.Core.Domain
{
    public class Client
    {
        public Client()
        {
            Logsheet = new HashSet<Logsheet>();
        }

        public int Id { get; set; }
        public string ClientPublicId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
        public string Name { get; set; } = default!;
        public AppEnums.ClientType Type { get; set; }

        // ignored properties:
        public bool IsValid { get; set; }
        public string? Platform { get; set; }
        public string? Browser { get; set; }
        public string? IP { get; set; }


        public virtual ICollection<Logsheet> Logsheet { get; set; }
    }
}
