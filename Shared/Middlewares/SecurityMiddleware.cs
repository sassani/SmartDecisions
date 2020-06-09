using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Shared.Response;

namespace Shared.Middlewares
{
    public class SecurityMiddleware
    {
        public static void UseSharedApiKey(IApplicationBuilder app, string sharedApiKey)
        {
            app.Use(async (context, next) =>
            {
                Microsoft.Extensions.Primitives.StringValues apiKey = "";
                var res = context.Response;
                Error error = new Error
                {
                    Code = "00000000",
                    Title = "Authorization Failed"
                };
                try
                {
                    if (!context.Request.Headers.TryGetValue("XXX_SHARED_API_KEY", out apiKey)) throw new Exception("You need XXX_SHARED_API_KEY header to access this API");
                    if (apiKey.ToString() != sharedApiKey) throw new Exception("XXX_SHARED_API_KEY header value is wrong");
                    
                    await next.Invoke();
                }
                catch (Exception err)
                {
                    res.StatusCode = (int)HttpStatusCode.Unauthorized;
                    res.Headers.Append(HeaderNames.ContentType, "application/json");
                    error.Detail = err.Message;
                    await context.Response.WriteAsync(new Response.Response(HttpStatusCode.Unauthorized, error).ToString());
                }
            });
        }
    }
}
