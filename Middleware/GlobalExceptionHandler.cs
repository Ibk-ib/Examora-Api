using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UniversityAcademicApi.Exceptions;


namespace UniversityAcademicApi.Middleware;

public class GlobalExceptionHandler 
    (ILogger<GlobalExceptionHandler> logger) : IExceptionHandler {


    public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "An unhandled exception occurred. TraceId: {TraceId}",
            httpContext.TraceIdentifier);

        if (httpContext.Response.HasStarted)
        {
            return false;
        }

        var (statusCode, title, detail) = exception switch
        {
            NotFoundException =>
                (StatusCodes.Status404NotFound,
                 "Resource Not Found",
                 exception.Message),

            ConflictException =>
                (StatusCodes.Status409Conflict,
                 "Conflict",
                 exception.Message),

            UnauthorizedAccessException =>
                (StatusCodes.Status401Unauthorized,
                 "Unauthorized",
                 "Invalid credentials or unauthorized access."),

            _ =>
                (StatusCodes.Status500InternalServerError,
                 "Internal Server Error",
                 "An unexpected error occurred.")
        };

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["TraceId"] =
            httpContext.TraceIdentifier;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }






}