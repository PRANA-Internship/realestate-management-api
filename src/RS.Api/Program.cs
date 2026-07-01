using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RS.Application.Common.Interfaces;
using RS.Infrastructure.Persistence;
using RS.Infrastructure.Persistence.Repositories;
using RS.Infrastructure.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<RSDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Configure MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RS.Application.Features.Auth.Commands.Login.LoginCommand).Assembly));



// Configure Custom Services and Repositories
builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<RSDbContext>());
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService, JwtProvider>();
builder.Services.AddScoped<IEmailService, EmailService>();

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





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
