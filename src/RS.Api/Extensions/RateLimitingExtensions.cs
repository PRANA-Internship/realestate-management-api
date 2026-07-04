using System.Security.Claims;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RS.Api.Extensions;

public static class RateLimitingExtensions
{
    private static readonly Dictionary<string, string> Policies = new()
    {
        ["login"] = "RateLimiting:Login",
        ["register"] = "RateLimiting:Register",
        ["authenticated"] = "RateLimiting:Authenticated",

        ["property-create"] = "RateLimiting:PropertyCreate",
        ["property-update"] = "RateLimiting:PropertyUpdate",
        ["property-delete"] = "RateLimiting:PropertyDelete",

        ["reservation"] = "RateLimiting:Reservation",
        ["purchase"] = "RateLimiting:Purchase",
        ["payment"] = "RateLimiting:Payment",

        ["admin"] = "RateLimiting:Admin"
    };

    public static IServiceCollection AddApplicationRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.ContentType = "application/json";

                await context.HttpContext.Response.WriteAsJsonAsync(
                    new
                    {
                        Error = new
                        {
                            Code = "RATE_LIMIT_001",
                            Message = "Too many requests. Please try again later."
                        }
                    },
                    cancellationToken);
            };

            foreach (var policy in Policies)
            {
                AddPolicy(
                    options,
                    configuration,
                    policy.Key,
                    policy.Value);
            }
        });

        return services;
    }

    private static void AddPolicy(
        RateLimiterOptions options,
        IConfiguration configuration,
        string policyName,
        string configurationPath)
    {
        var permitLimit = configuration.GetValue<int>($"{configurationPath}:PermitLimit");
        if (permitLimit <= 0)
        {
            permitLimit = 100;
        }

        var windowSeconds = configuration.GetValue<int>($"{configurationPath}:WindowSeconds");
        if (windowSeconds <= 0)
        {
            windowSeconds = 60;
        }

        options.AddPolicy(policyName, httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
                GetPartitionKey(httpContext),
                _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = permitLimit,
                    Window = TimeSpan.FromSeconds(windowSeconds),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = true
                }));
    }

    private static string GetPartitionKey(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId =
                context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                context.User.FindFirst("sub")?.Value ??
                context.User.Identity?.Name;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                return $"user:{userId}";
            }
        }

        var ip = context.Connection.RemoteIpAddress?.ToString();

        return !string.IsNullOrWhiteSpace(ip)
            ? $"ip:{ip}"
            : "anonymous";
    }
}
