using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using RS.Api.Extensions;
using RS.API.Middleware;

using RS.Application.Common.Interfaces;
using RS.Infrastructure.Persistence;
using RS.Infrastructure.Persistence.Repositories;
using RS.Infrastructure.Services;
using UMS.Application.Common.Behaviours;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApplicationRateLimiting(builder.Configuration);
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();



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



// Configure Custom Services and Repositories
builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<RSDbContext>());
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService, JwtProvider>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddHttpClient<IStorageService, SupabaseStorageService>();
builder.Services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddHostedService<ReservationExpirationService>();
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


// Configure JWT Authentication
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"] ?? "super_secret_key_12345678901234567890"))
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

        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        await DatabaseSeeder.SeedAsync(dbContext, configuration);

        await ConfigurationSeeder.SeedAsync(dbContext);

        logger.LogInformation("Database migration completed successfully!");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database.");
        throw; // Re-throw



    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRateLimiter();
app.UseAuthorization();

app.MapControllers();

app.Run();
