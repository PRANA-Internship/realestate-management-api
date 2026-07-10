using Microsoft.EntityFrameworkCore;
using RS.Domain.Entities;
using RS.Domain.Enums;

namespace RS.Infrastructure.Persistence;

public static class ConfigurationSeeder
{
    public static async Task SeedAsync(RSDbContext dbContext)
    {
        var currentConfigurations = new List<SystemConfiguration>
{

    new()
    {
        Key = "Reservation.Enabled",
        Value = "true",
        DefaultValue = "true",
        DataType = ConfigDataType.Boolean,
        Description = "Enable reservation feature"
    },
    new()
    {
        Key = "Reservation.Fee",
        Value = "10000",
        DefaultValue = "10000",
        DataType = ConfigDataType.Decimal,
        Description = "Fixed reservation fee in ETB"
    },
    new()
    {
        Key = "Reservation.DurationHours",
        Value = "48",
        DefaultValue = "48",
        DataType = ConfigDataType.Integer,
        Description = "Reservation validity in hours"
    },
    new()
    {
        Key = "Reservation.MaximumPerBuyer",
        Value = "3",
        DefaultValue = "3",
        DataType = ConfigDataType.Integer,
        Description = "Maximum active reservations per buyer"
    },
    new()
    {
        Key = "Reservation.AutoExpire",
        Value = "true",
        DefaultValue = "true",
        DataType = ConfigDataType.Boolean,
        Description = "Automatically expire reservations"
    },
    new()
    {
        Key = "Reservation.ExpirationCheckIntervalSeconds",
        Value = "300",
        DefaultValue = "300",
        DataType = ConfigDataType.Integer,
        Description = "Interval in seconds used to check and expire unpaid reservations."
    },


    new()
    {
        Key = "Property.MaxImages",
        Value = "10",
        DefaultValue = "10",
        DataType = ConfigDataType.Integer,
        Description = "Maximum images per property"
    },
    new()
    {
        Key = "Property.MaxImageSizeMB",
        Value = "5",
        DefaultValue = "5",
        DataType = ConfigDataType.Integer,
        Description = "Maximum image size in MB"
    },
    new()
    {
        Key = "Property.AllowDeactivation",
        Value = "true",
        DefaultValue = "true",
        DataType = ConfigDataType.Boolean,
        Description = "Allow property deactivation"
    },


    new()
    {
        Key = "Payment.MaximumRetries",
        Value = "3",
        DefaultValue = "3",
        DataType = ConfigDataType.Integer,
        Description = "Maximum payment retries"
    },
    new()
    {
        Key = "Payment.TimeoutMinutes",
        Value = "15",
        DefaultValue = "15",
        DataType = ConfigDataType.Integer,
        Description = "Payment timeout in minutes"
    },


    new()
    {
        Key = "User.RequireEmailVerification",
        Value = "true",
        DefaultValue = "true",
        DataType = ConfigDataType.Boolean,
        Description = "Require email verification"
    },
    new()
    {
        Key = "User.PasswordResetExpiryMinutes",
        Value = "50",
        DefaultValue = "50",
        DataType = ConfigDataType.Integer,
        Description = "Password reset expiry time"
    },


    new()
    {
        Key = "Email.SendWelcome",
        Value = "true",
        DefaultValue = "true",
        DataType = ConfigDataType.Boolean,
        Description = "Send welcome email"
    },
    new()
    {
        Key = "Email.SendPasswordReset",
        Value = "true",
        DefaultValue = "true",
        DataType = ConfigDataType.Boolean,
        Description = "Send password reset email"
    }
};

        bool changesMade = false;

        foreach (var seedConfig in currentConfigurations)
        {
            var existing = await dbContext.SystemConfigurations
                .FirstOrDefaultAsync(c => c.Key == seedConfig.Key);

            if (existing == null)
            {
                seedConfig.Id = Guid.NewGuid();
                seedConfig.CreatedAt = DateTime.UtcNow;

                await dbContext.SystemConfigurations.AddAsync(seedConfig);
                changesMade = true;
            }
            else
            {
                bool updated = false;

                if (existing.DefaultValue != seedConfig.DefaultValue)
                {
                    existing.DefaultValue = seedConfig.DefaultValue;
                    updated = true;
                }

                if (existing.DataType != seedConfig.DataType)
                {
                    existing.DataType = seedConfig.DataType;
                    updated = true;
                }

                if (existing.Description != seedConfig.Description)
                {
                    existing.Description = seedConfig.Description;
                    updated = true;
                }



                if (updated)
                {
                    existing.UpdatedAt = DateTime.UtcNow;
                    changesMade = true;
                }
            }
        }

        if (changesMade)
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
