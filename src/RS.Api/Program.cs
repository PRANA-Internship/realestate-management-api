using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using RS.API.Middleware;

using RS.Application.Common.Interfaces;
using RS.Infrastructure.Persistence.Migrations;
using RS.Infrastructure.Persistence.Repositories;
using RS.Infrastructure.Persistence.Seed;
using RS.Infrastructure.Services;
using UMS.Application.Common.Behaviours;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// DbContext (PostgreSQL)
builder.Services.AddDbContext<RSDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure MediatR
// 1. Automatically finds and registers every single validator in your application assembly
builder.Services.AddValidatorsFromAssembly(typeof(RS.Application.Features.Auth.Commands.Login.LoginCommand).Assembly);


// 2. Add MediatR alongside the automated validation behavior pipeline 
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RS.Application.Features.Auth.Commands.Login.LoginCommand).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
});


// 2. Add MediatR alongside the automated validation behavior pipeline 
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RS.Application.Features.Auth.Commands.Login.LoginCommand).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
});


// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RS.Application.Features.Auth.Commands.Login.LoginCommand).Assembly));

// DI
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<RSDbContext>());
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService, JwtProvider>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "RS API", Version = "v1" });

    // 1. Define the Security Scheme object using HTTP Bearer standards
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter your JWT token directly. Example: 'your_token_here'"
    });


    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>() // Explicitly match the List<string> signature
        }
    });
});



builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "RS API", Version = "v1" });

    // 1. Define the Security Scheme object using HTTP Bearer standards
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter your JWT token directly. Example: 'your_token_here'"
    });


    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>() // Explicitly match the List<string> signature
        }
    });
});


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



app.UseMiddleware<GlobalExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RSDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Starting database migration...");

        // Ensure database is created and migrations are applied
        dbContext.Database.Migrate();

        logger.LogInformation("Database migration completed successfully!");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database.");
        throw; // Re-throw



    }
}



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
