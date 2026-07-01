using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace RS.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Try to execute the request normally
            await _next(context);
        }
        catch (OperationCanceledException)
        {
            // Request was aborted/cancelled; do not treat as an application error.
            _logger.LogInformation("Request was canceled. TraceId: {TraceId}", context.TraceIdentifier);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (ArgumentNullException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (ArgumentException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (KeyNotFoundException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (DbUpdateException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (FluentValidation.ValidationException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (InvalidOperationException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (SystemException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Log the exception
        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        // Set response details
        context.Response.ContentType = "application/json";

        // Map exception to HTTP status code
        context.Response.StatusCode = exception switch
        {
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            ArgumentException or ArgumentNullException => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            DbUpdateConcurrencyException => (int)HttpStatusCode.Conflict,
            DbUpdateException => (int)HttpStatusCode.InternalServerError,
            FluentValidation.ValidationException => (int)HttpStatusCode.BadRequest,
            InvalidOperationException => (int)HttpStatusCode.InternalServerError,
            SystemException => (int)HttpStatusCode.InternalServerError,
            _ => (int)HttpStatusCode.InternalServerError
        };

        // Build error response
        var errorResponse = new
        {
            StatusCode = context.Response.StatusCode,
            Message = _env.IsDevelopment()
                ? exception.Message
                : "An unexpected error occurred. Please try again later.",
            Detail = _env.IsDevelopment() ? exception.StackTrace : null,
            Timestamp = DateTime.UtcNow,
            TraceId = context.TraceIdentifier,
            // Include inner exception details in development
            InnerException = _env.IsDevelopment() ? exception.InnerException?.Message : null
        };

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}
