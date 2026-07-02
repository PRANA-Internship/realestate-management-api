using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RS.Application.Common.Interfaces;
using RS.Infrastructure.Persistence.Migrations;
using RS.Infrastructure.Persistence.Repositories;
using RS.Infrastructure.Persistence.Seed;
using RS.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// DbContext (PostgreSQL)
builder.Services.AddDbContext<RSDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RS.Application.Features.Auth.Commands.Login.LoginCommand).Assembly));

// DI
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<RSDbContext>());
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService, JwtProvider>();
builder.Services.AddScoped<IEmailService, EmailService>();

// JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["JwtSettings:Secret"]
                    ?? "super_secret_key_12345678901234567890"))
        };
    });

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// ===============================
// SAFE DATABASE SEEDING
// ===============================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RSDbContext>();

    try
    {
        await PermissionSeeder.SeedAsync(context);
        Console.WriteLine("✅ Permission seeding completed");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Seeding failed:");
        Console.WriteLine(ex.Message);
    }
}

app.Run();
