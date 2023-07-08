using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Shared.Response;

namespace Shared.Middlewares
{
    public class RequestSharedApiKeyMiddleware
    {
        private static readonly string SHARED_HEADER_NAME = GLOBAL_CONSTANTS.SHARED_API_KEY_HEADER_NAME;
        private readonly RequestDelegate next;
        private readonly string sharedApiKey;
        public RequestSharedApiKeyMiddleware(RequestDelegate next, string sharedApiKey)
        {
            this.next = next;
            this.sharedApiKey = sharedApiKey;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // Call the next delegate/middleware in the pipeline

            Microsoft.Extensions.Primitives.StringValues apiKey = "";
            var res = context.Response;
            Error error = new Error
            {
                Code = "00000000",
                Title = "Authorization Failed"
            };
            try
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower() != "development")
                {

                    if (!context.Request.Headers.TryGetValue(SHARED_HEADER_NAME, out apiKey)) throw new Exception($"You need {SHARED_HEADER_NAME} header to access this API");
                    if (apiKey.ToString() != sharedApiKey) throw new Exception($"{SHARED_HEADER_NAME} header value is wrong");
                }

                await next(context);
            }
            catch (Exception err)
            {
                res.StatusCode = (int)HttpStatusCode.Unauthorized;
                res.Headers.Append(HeaderNames.ContentType, "application/json");
                error.Detail = err.Message;
                await context.Response.WriteAsync(new Response.Response(HttpStatusCode.Unauthorized, error).ToString());
            }
        }
    }
}
