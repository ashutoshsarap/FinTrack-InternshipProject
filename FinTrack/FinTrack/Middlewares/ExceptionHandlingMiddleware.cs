using FinTrack.CustomExceptions;
using FinTrack.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Threading.Tasks;

namespace FinTrack.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandlingMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext);
            }
            catch(Exception ex)
            {

                var statusCode = GetStatusCode(ex);

                _logger.LogError(ex, $"Some unhandled exception, Path : {httpContext.Request.Path}, Method : {httpContext.Request.Method}", httpContext.Request.Path, httpContext.Request.Method);
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(new ErrorResponse
                {
                    Message = "Some error occurred",
                    StatusCode = statusCode
                });
            }
        }

        private static int GetStatusCode(Exception ex)
        {
            return ex switch
            {
                DuplicateRecordException => StatusCodes.Status409Conflict,
                InvalidAmountException => StatusCodes.Status400BadRequest,
                InvalidDateException => StatusCodes.Status400BadRequest,
                RecordNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
        }
        
    }
    


    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
