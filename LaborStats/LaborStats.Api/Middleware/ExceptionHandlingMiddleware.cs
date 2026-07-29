
using System.Net;
using FluentValidation;
using LaborStats.Application.Imports;
using LaborStats.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LaborStats.Api.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionHandlingMiddleware(
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                var requestPath = $"{context.Request.Method} {context.Request.Path}";

                switch (e)
                {
                    case ValidationException validationException:
                        var errors = string.Join(" | ", validationException.Errors
                            .Select(err => $"{err.PropertyName}: {err.ErrorMessage}"));
                        _logger.LogWarning("Validation failed on {RequestPath}. Errors: {Errors}", requestPath, errors);
                        break;

                    case AppException ae:
                        _logger.LogWarning(ae, "Handled domain exception on {RequestPath}: {Message}", requestPath, ae.Message);
                        break;

                    default:
                        _logger.LogError(e, "Unhandled exception on {RequestPath}", requestPath);
                        break;
                }

                await HandleExceptionAsync(context, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, title) = MapException(exception);
            var traceId = context.TraceIdentifier;

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;

            if (exception is ImportValidationException importValidationException)
            {
                var problemDetailsWithErrors = new ProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Detail = exception.Message,
                    Instance = context.Request.Path,
                    Type = $"https://httpstatuses.io/{statusCode}"
                };

                problemDetailsWithErrors.Extensions["traceId"] = traceId;
                problemDetailsWithErrors.Extensions["errors"] = importValidationException.Errors;

                await context.Response.WriteAsJsonAsync(problemDetailsWithErrors);
                return;
            }

            if (exception is ValidationException fluentValidationException)
            {
                var validationErrors = fluentValidationException.Errors
                    .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                    .ToDictionary(g => g.Key, g => g.ToArray());

                var validationProblemDetails = new ValidationProblemDetails(validationErrors)
                {
                    Status = statusCode,
                    Title = title,
                    Detail = exception.Message,
                    Instance = context.Request.Path,
                    Type = $"https://httpstatuses.io/{statusCode}"
                };
                validationProblemDetails.Extensions["traceId"] = traceId;

                await context.Response.WriteAsJsonAsync(validationProblemDetails);
                return;
            }

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = _environment.IsDevelopment() || statusCode < (int)HttpStatusCode.InternalServerError
                    ? exception.Message
                    : "An unexpected server error occurred.",
                Instance = context.Request.Path,
                Type = $"https://httpstatuses.io/{statusCode}"
            };

            problemDetails.Extensions["traceId"] = traceId;

            if (_environment.IsDevelopment() && statusCode >= (int)HttpStatusCode.InternalServerError)
            {
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            }

            await context.Response.WriteAsJsonAsync(problemDetails);
        }

        private static (int StatusCode, string Title) MapException(Exception exception) => exception switch
        {
            NotFoundException => ((int)HttpStatusCode.NotFound, "Resource not found"),
            ConflictException => ((int)HttpStatusCode.Conflict, "Conflict"),
            ImportValidationException => ((int)HttpStatusCode.BadRequest, "Import validation failed"),
            UnauthorizedException => ((int)HttpStatusCode.Unauthorized, "Unauthorized"),
            ValidationException => ((int)HttpStatusCode.BadRequest, "Validation error"),
            _ => ((int)HttpStatusCode.InternalServerError, "An unexpected server error occured")
        };
    }
}
