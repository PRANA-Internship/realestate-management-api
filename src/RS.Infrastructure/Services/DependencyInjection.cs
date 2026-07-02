using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RS.Infrastructure.Configurations;

namespace RS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register other infrastructure services here
        // services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
