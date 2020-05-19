using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Shared.ErrorHandlers
{
    public class TokenException:HttpResponseException
    {
        public TokenException(HttpStatusCode status = HttpStatusCode.Unauthorized, string title = null, string description = null)
        {
            Status = status;
            Title = title;
            Description = description;
        }
    }
}
