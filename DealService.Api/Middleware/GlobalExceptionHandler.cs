using DealService.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace DealService.Api.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, exception.Message);

            var statusCode = exception switch
            {
               InvalidStatusTransactionException => StatusCodes.Status400BadRequest,
               UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
               _ => StatusCodes.Status500InternalServerError
            };

            var response = new ProblemDetails
            {
                Status = statusCode,
                Title = "Error occurred",
                Detail = exception.Message,
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }
    }
}
