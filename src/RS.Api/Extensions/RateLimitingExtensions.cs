using System;
using System.Collections.Generic;
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
        ["authentication"] = "RateLimiting:Authentication",
        ["action"] = "RateLimiting:Action",
        ["public"] = "RateLimiting:Public"
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
            permitLimit = 100; // Safe fallback
        }

        var windowSeconds = configuration.GetValue<int>($"{configurationPath}:WindowSeconds");
        if (windowSeconds <= 0)
        {
            windowSeconds = 60; // Safe fallback
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

        var ip = context.Connection.RemoteIpAddress?.ToString()
                 ?? context.Request.Headers["X-Forwarded-For"].ToString();

        return !string.IsNullOrWhiteSpace(ip) ? $"ip:{ip}" : "anonymous";
    }
}
