using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;
//V1
namespace FinTrack.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomLoggingMiddleware> _logger;
        public CustomLoggingMiddleware(RequestDelegate next, ILogger<CustomLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            _logger.LogInformation("Request hitted \nRequest Path: {Path} \nMethod: {Method}", httpContext.Request.Path, httpContext.Request.Method);
            Stopwatch stopwatch = Stopwatch.StartNew();
            await _next(httpContext);
            stopwatch.Stop();
            _logger.LogInformation("Response Status Code: {StatusCode} \nTime taken to complete request : {TimeEllapsed}ms", httpContext.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomLoggingMiddleware>();
        }
    }
}
