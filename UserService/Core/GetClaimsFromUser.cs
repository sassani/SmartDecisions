using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DecissionCore.Core
{
    public class GetClaimsFromUser : IGetClaimsProvider
    {
        public GetClaimsFromUser(IHttpContextAccessor accessor)
        {
            UserId = accessor?.HttpContext?.User?.Claims?.SingleOrDefault(c => c.Type == "uid")?.Value;
        }

        public string UserId { get; private set; }
    }
}
