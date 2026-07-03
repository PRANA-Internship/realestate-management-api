using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RS.Domain.Entities;
using RS.Domain.Enums;

namespace RS.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(
        RSDbContext context,
        IConfiguration configuration)
    {
        await SeedAdminAsync(context, configuration);

        await SeedManagerAsync(context, configuration);

        await context.SaveChangesAsync();
    }

    private static async Task SeedAdminAsync(
        RSDbContext context,
        IConfiguration configuration)
    {
        var email = configuration["SeedUsers:Admin:Email"];

        if (await context.Users.AnyAsync(x => x.Email == email))
            return;

        var admin = User.CreateStaff(
            configuration["SeedUsers:Admin:FullName"]!,
            email!,
            configuration["SeedUsers:Admin:Phone"]!,
            UserRole.ADMIN);

        admin.ActivateWithPassword(
            BCrypt.Net.BCrypt.HashPassword(
                configuration["SeedUsers:Admin:Password"]!));

        await context.Users.AddAsync(admin);
    }

    private static async Task SeedManagerAsync(
        RSDbContext context,
        IConfiguration configuration)
    {
        var email = configuration["SeedUsers:Manager:Email"];

        if (await context.Users.AnyAsync(x => x.Email == email))
            return;

        var manager = User.CreateStaff(
            configuration["SeedUsers:Manager:FullName"]!,
            email!,
            configuration["SeedUsers:Manager:Phone"]!,
            UserRole.MANAGER);

        manager.ActivateWithPassword(
            BCrypt.Net.BCrypt.HashPassword(
                configuration["SeedUsers:Manager:Password"]!));

        await context.Users.AddAsync(manager);
    }
}
