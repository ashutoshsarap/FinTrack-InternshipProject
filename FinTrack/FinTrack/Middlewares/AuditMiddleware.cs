using FinTrack.Data;
using FinTrack.Models.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
//V2
namespace FinTrack.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditMiddleware> _logger;
        private ApplicationDbContext _dbContext;
        public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext, ApplicationDbContext dbContext)
        {

            await _next(httpContext);

            _dbContext = dbContext;

            if ((httpContext.Request.Method == HttpMethods.Post) || 
                (httpContext.Request.Method==HttpMethods.Put) || 
                (httpContext.Request.Method==HttpMethods.Delete))
            {
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
                    RequestPath = httpContext.Request.Path,
                    Method = httpContext.Request.Method,
                    ResponseStatusCode = httpContext.Response.StatusCode,
                    Timestamp = DateTime.UtcNow
                };

                await _dbContext.AuditLogs.AddAsync(auditData);
                await _dbContext.SaveChangesAsync();
            }


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
