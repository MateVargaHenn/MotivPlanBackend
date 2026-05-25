using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FluentValidation;

namespace MotivPlanBackend.WebApi.Handlers;

internal sealed partial class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;
    private static readonly Action<ILogger, Exception> _logUnhandledException =
    LoggerMessage.Define(
        LogLevel.Error,
        new EventId(0, nameof(LogUnhandledException)),
        "An unhandled exception occurred.");
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException)
        {
            LogUnhandledException(exception);
        }

        ProblemDetails problemDetails = exception switch
        {
            ValidationException validationException => CreateValidationProblemDetails(validationException),

            ArgumentNullException => CreateProblemDetails(
                "Bad Request",
                HttpStatusCode.BadRequest,
                "One or more required arguments were null.",
                exception.Message),

            ArgumentException => CreateProblemDetails(
                "Bad Request",
                HttpStatusCode.BadRequest,
                "One or more arguments were invalid.",
                exception.Message),

            InvalidOperationException => CreateProblemDetails(
                "Bad Request",
                HttpStatusCode.BadRequest,
                "The operation is invalid.",
                exception.Message),

            UnauthorizedAccessException => CreateProblemDetails(
                "Unauthorized",
                HttpStatusCode.Unauthorized,
                "You are not authorized to access this resource.",
                exception.Message),

            KeyNotFoundException => CreateProblemDetails(
                "Not Found",
                HttpStatusCode.NotFound,
                "The requested resource was not found.",
                exception.Message),

            _ => CreateProblemDetails(
                "Internal Server Error",
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred.",
                "An internal server error occurred. Please try again later.")
        };

        httpContext.Response.StatusCode = (int)problemDetails.Status!;
        await httpContext.Response.WriteAsJsonAsync((object)problemDetails, cancellationToken);

        return true;
    }
    private void LogUnhandledException(Exception exception)
    {
        _logUnhandledException(_logger, exception);
    }
    private static ProblemDetails CreateProblemDetails(
        string title,
        HttpStatusCode statusCode,
        string detail,
        string instance) =>
        new()
        {
            Title = title,
            Status = (int)statusCode,
            Detail = detail,
            Instance = instance
        };

    private static ValidationProblemDetails CreateValidationProblemDetails(
    ValidationException validationException)
    {
        var errors = validationException.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(error => error.ErrorMessage)
                    .ToArray());

        return new ValidationProblemDetails(errors)
        {
            Title = "Validation failed",
            Status = StatusCodes.Status400BadRequest,
            Detail = "One or more validation errors occurred."
        };
    }
}