using System;
using System.Net;

namespace IntraServicesApi
{
    public class IntraServiceException : Exception
    {
        public HttpStatusCode Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IntraServiceException(HttpStatusCode status = HttpStatusCode.InternalServerError, string title = null, string description = null)
        {
            Status = status;
            Title = title;
            Description = description;
        }
    }
}
