using Microsoft.AspNetCore.Builder;

namespace Shared.Middlewares.Extensions
{
    public static class RequestSharedApiKeyMiddlewareExtension
    {
        public static IApplicationBuilder UseRequestSharedApiKey(
            this IApplicationBuilder builder, string sharedApiKey)
        {
            return builder.UseMiddleware<RequestSharedApiKeyMiddleware>(sharedApiKey);
        }
    }
}
