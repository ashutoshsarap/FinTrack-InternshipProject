using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
//V1
namespace FinTrack.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditMiddleware> _logger;
        public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            await _next(httpContext);

            _logger.LogInformation("Audit Log - Request Path: {Path}, Method: {Method}, Response Status Code: {StatusCode}, Date and Time: {DateTime}",
                httpContext.Request.Path,
                httpContext.Request.Method,
                httpContext.Response.StatusCode,
                DateTime.UtcNow);

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuditMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuditMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuditMiddleware>();
        }
    }
}
