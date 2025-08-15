using Microsoft.AspNetCore.Diagnostics;
using OAuthWebApi.Domain.Exceptions;
using System.Net;

namespace OAuthWebApi.Handler
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
            var (statusCode, message) = GetExceptionDetails(exception);

            _logger.LogError(exception, exception.Message);

            httpContext.Response.StatusCode = (int)statusCode;
            await httpContext.Response.WriteAsJsonAsync(message, cancellationToken);

            return true;
        }

        private (HttpStatusCode statusCode, string message) GetExceptionDetails(Exception exception)
        {
            return exception switch
            {
                LoginFailedException => (HttpStatusCode.Unauthorized, exception.Message),
                UserAlreadyExistsException => (HttpStatusCode.Conflict, exception.Message),
                RegistrationFailedException => (HttpStatusCode.BadRequest, exception.Message),
                RefreshTokenException => (HttpStatusCode.Unauthorized, exception.Message),
                _ => (HttpStatusCode.InternalServerError, $"An unexpected error occurred: {exception.Message}")
            };
        }
    }
}
