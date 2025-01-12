using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace BuildingBlocks.Exceptions.Handler
{
    public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError("Error Message: {exceptionMessage}, Time of occurrence {time}", exception.Message, DateTime.UtcNow);
            (string Details, string Title, int StatusCode) details = exception switch
            {
                InternalServerException =>
                (
                exception.Message,
                 exception.GetType().Name,
                 context.Response.StatusCode = StatusCodes.Status500InternalServerError
                 ),
                ValidationException =>
                (exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                BadRequestException =>
                (
                exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                NotFoundException =>
                (
                exception.Message,
                   exception.GetType().Name,
                   context.Response.StatusCode = StatusCodes.Status404NotFound
                ),
                _ => (
                exception.Message,
                exception.GetType().Name,
                  context.Response.StatusCode = StatusCodes.Status500InternalServerError
                )
            };

            var problemDetails = new ProblemDetails
            {
                Detail = details.Details,
                Title = details.Title,
                Status = details.StatusCode,
                Instance = context.Request.Path
            };
            problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

            if(exception is ValidationException validationException)
            {
                problemDetails.Extensions.Add("Validation Errors", validationException.Errors);
            }
            await context.Response.WriteAsJsonAsync( problemDetails,cancellationToken:cancellationToken );
            return true;

        }
    }
}
