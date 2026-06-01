using FinTrack.CustomExceptions;
using FinTrack.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace FinTrack.Handler
{
    public class GlobalExceptionHandler : IExceptionHandler
    {

        private readonly ILogger _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred.\nPath : {Path}\nMethod : {Method}", httpContext.Request.Path, httpContext.Request.Method);

            httpContext.Response.StatusCode = GetStatusCode(exception);

            var errorMessage = GetErrorMessage(exception);

            await httpContext.Response.WriteAsJsonAsync(new ErrorResponse
            {
                Message = errorMessage,
                StatusCode = httpContext.Response.StatusCode
            }, cancellationToken);
            return true;
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

        private static string GetErrorMessage(Exception ex)
        {
            var message = ex switch
            {
                DuplicateRecordException => ex.Message,
                InvalidAmountException => ex.Message,
                InvalidDateException => ex.Message,
                RecordNotFoundException => ex.Message,
                _ => "An unexpected error occurred."
            };

            return message;
        }
    }
}
