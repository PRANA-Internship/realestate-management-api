using Microsoft.EntityFrameworkCore;
using RS.Domain.Entities;

namespace RS.Infrastructure.Persistence;

public static class ConfigurationSeeder
{
    public static async Task SeedAsync(RSDbContext dbContext)
    {
        if (await dbContext.SystemConfigurations.AnyAsync())
        {
            return;
        }

        var configurations = new List<SystemConfiguration>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Key = "Reservation.Enabled",
                Value = "true",
                Description = "Enable reservation feature"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Key = "Reservation.Fee",
                Value = "10000",
                Description = "Fixed reservation fee in ETB"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Key = "Reservation.DurationHours",
                Value = "48",
                Description = "Reservation validity in hours"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Key = "Reservation.MaximumPerBuyer",
                Value = "3",
                Description = "Maximum active reservations per buyer"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Key = "Reservation.AutoExpire",
                Value = "true",
                Description = "Automatically expire reservations"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Key = "Property.MaxImages",
                Value = "10",
                Description = "Maximum images per property"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Key = "Property.MaxImageSizeMB",
                Value = "5",
                Description = "Maximum image size in MB"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Key = "Property.AllowDeactivation",
                Value = "true",
                Description = "Allow property deactivation"
            },

            new()
            {
                Id = Guid.NewGuid(),
                Key = "Payment.MaximumRetries",
                Value = "3",
                Description = "Maximum payment retries"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Key = "User.RequireEmailVerification",
                Value = "true",
                Description = "Require email verification"
            },
        };

        await dbContext.SystemConfigurations.AddRangeAsync(configurations);

        await dbContext.SaveChangesAsync();
    }
}
