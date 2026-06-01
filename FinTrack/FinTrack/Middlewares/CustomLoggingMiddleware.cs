using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;
//V2
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
            _logger.LogInformation("Request started " +
                                    "Request Id : {TraceIdentifier} " +
                                   "\nRequest Path: {Path} " +
                                   "\nMethod: {Method}" +
                                   "\nHost : {Host}" +
                                   "\nUser-Agent : {UserAgent}", 
                                    httpContext.TraceIdentifier, //Gets or sets a unique identifier to represent this request in trace logs.
                                    httpContext.Request.Path, 
                                    httpContext.Request.Method,
                                    httpContext.Request.Host,
                                    httpContext.Request.Headers.UserAgent); //a software program (like a web browser, email client, or web crawler) that acts as a user's representative in a network
            Stopwatch stopwatch = Stopwatch.StartNew();
            await _next(httpContext);
            stopwatch.Stop();
            if(stopwatch.ElapsedMilliseconds > 1000)
            {
                _logger.LogWarning("Request took more than expecteds" +
                                    "\nRequest Path: {Path}" +
                                    "\nMethod: {Method}" +
                                    "\nResponse Status Code : {StatusCode}" +
                                    "\nContent type : {ContentType}" +
                                    "\nDate and Time : {DateTime}" +
                                    "\nTime taken to complete request : {TimeEllapsed}ms",
                                    httpContext.Request.Path,
                                    httpContext.Request.Method,
                                    httpContext.Response.StatusCode,
                                    httpContext.Response.ContentType,
                                    DateTime.UtcNow,
                                    stopwatch.ElapsedMilliseconds);
            }
            _logger.LogInformation("Request completed" +
                                    "\nRequest Path: {Path}" +
                                    "\nMethod: {Method}" +
                                    "\nResponse Status Code : {StatusCode}" +
                                    "\nContent type : {ContentType}" +
                                    "\nDate and Time : {DateTime}" +
                                    "\nTime taken to complete request : {TimeEllapsed}ms",
                                    httpContext.Request.Path,
                                    httpContext.Request.Method,
                                    httpContext.Response.StatusCode,
                                    httpContext.Response.ContentType,
                                    DateTime.UtcNow,
                                    stopwatch.ElapsedMilliseconds);
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
