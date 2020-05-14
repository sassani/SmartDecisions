using System;
using System.Net;

namespace Shared.ErrorHandlers
{
    public class HttpResponseException : Exception
    {
        public HttpStatusCode Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public HttpResponseException(HttpStatusCode status = HttpStatusCode.InternalServerError, string title = null, string description = null)
        {
            Status = status;
            Title = title;
            Description = description;
        }

    }
}
