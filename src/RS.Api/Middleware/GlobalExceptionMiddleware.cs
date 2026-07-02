using System.Net;
using System.Text.Json;

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
            await _next(context);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Request was canceled. TraceId: {TraceId}", context.TraceIdentifier);
        }
        catch (Exception ex)
        {
            // Catch ALL other exceptions here and let the handler deal with them
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        context.Response.ContentType = "application/json";

        // Map exception using Reflection/Type Names to avoid referencing other layers
        context.Response.StatusCode = GetStatusCode(exception);

        var errorResponse = new
        {
            StatusCode = context.Response.StatusCode,
            Message = _env.IsDevelopment()
                ? exception.Message
                : "An unexpected error occurred. Please try again later.",
            Detail = _env.IsDevelopment() ? exception.StackTrace : null,
            Timestamp = DateTime.UtcNow,
            TraceId = context.TraceIdentifier,
            InnerException = _env.IsDevelopment() ? exception.InnerException?.Message : null
        };

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }

    private static int GetStatusCode(Exception exception)
    {
        var exceptionType = exception.GetType().Name;

        return exception switch
        {
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            ArgumentException or ArgumentNullException => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            InvalidOperationException => (int)HttpStatusCode.InternalServerError,
            SystemException => (int)HttpStatusCode.InternalServerError,

            // Handle external layer exceptions by checking their Type Name as strings
            _ when exceptionType == "DbUpdateConcurrencyException" => (int)HttpStatusCode.Conflict,
            _ when exceptionType == "DbUpdateException" => (int)HttpStatusCode.InternalServerError,
            _ when exceptionType == "ValidationException" => (int)HttpStatusCode.BadRequest,

            _ => (int)HttpStatusCode.InternalServerError
        };
    }
}
