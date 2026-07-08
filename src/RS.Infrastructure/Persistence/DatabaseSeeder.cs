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

    // ---------------- ADMIN ----------------
    private static async Task SeedAdminAsync(
        RSDbContext context,
        IConfiguration configuration)
    {
        var email = configuration["SeedUsers:Admin:Email"]!;

        var admin = await context.Users
            .FirstOrDefaultAsync(x => x.Email == email);

        if (admin == null)
        {
            admin = User.CreateStaff(
                configuration["SeedUsers:Admin:FullName"]!,
                email,
                configuration["SeedUsers:Admin:Phone"]!,
                UserRole.ADMIN);

            admin.ActivateWithPassword(
                BCrypt.Net.BCrypt.HashPassword(
                    configuration["SeedUsers:Admin:Password"]!));

            await context.Users.AddAsync(admin);
        }
        else
        {
            admin.UpdateProfileSafe(
                configuration["SeedUsers:Admin:FullName"]!,
                configuration["SeedUsers:Admin:Phone"]!);
        }
    }

    // ---------------- MANAGER ----------------
    private static async Task SeedManagerAsync(
        RSDbContext context,
        IConfiguration configuration)
    {
        var email = configuration["SeedUsers:Manager:Email"]!;

        var manager = await context.Users
            .FirstOrDefaultAsync(x => x.Email == email);

        var admin = await context.Users
            .FirstAsync(x => x.Role == UserRole.ADMIN);

        if (manager == null)
        {
            manager = User.CreateStaff(
                configuration["SeedUsers:Manager:FullName"]!,
                email,
                configuration["SeedUsers:Manager:Phone"]!,
                UserRole.MANAGER);

            manager.SetCreatedBy(admin.Id);

            manager.ActivateWithPassword(
                BCrypt.Net.BCrypt.HashPassword(
                    configuration["SeedUsers:Manager:Password"]!));

            await context.Users.AddAsync(manager);
        }
        else
        {
            manager.UpdateProfileSafe(
                configuration["SeedUsers:Manager:FullName"]!,
                configuration["SeedUsers:Manager:Phone"]!);

            // ORPHAN ISSUE 
            //if (manager.CreatedByUserId == null)
            //{
            //    manager.SetCreatedBy(admin.Id);
            //}
        }
    }
}
