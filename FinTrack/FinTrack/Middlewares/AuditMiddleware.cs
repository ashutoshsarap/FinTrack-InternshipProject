using FinTrack.Data;
using FinTrack.Models.Entity;
using FinTrack.Service.IService;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
//V3
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

        public async Task Invoke(HttpContext httpContext, IAuditService auditService)
        {

            await _next(httpContext);

            var user = httpContext.User?.Identity?.Name;
            var action = httpContext.Items["AuditMessage"]?.ToString();

            //_logger.LogInformation("Audit Log - User : {User}, Action : {action}, Request Path: {Path}, Method: {Method}, Response Status Code: {StatusCode}, Date and Time: {DateTime}",
            //    user,
            //    action,
            //    httpContext.Request.Path,
            //    httpContext.Request.Method,
            //    httpContext.Response.StatusCode,
            //    DateTime.UtcNow);
            var auditData = new AuditData
            {
                UserName = user,
                Action = action,
                Timestamp = DateTime.UtcNow
            };
            
            BackgroundJob.Enqueue(() => auditService.LogAuditDataAsync(auditData));
        }

        private static string GetAction(string method)
        {
            var action = method switch
            {
                "POST" => "Created",
                "PUT" => "Updated",
                "DELETE" => "Deleted",
                _ => "Unknown"
            };

            return action;
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
