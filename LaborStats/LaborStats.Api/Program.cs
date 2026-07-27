using System.Text;
using FluentValidation;
using LaborStats.Api.Middleware;
using LaborStats.Api.OpenApi;
using LaborStats.Application.Roles;
using LaborStats.Infrastructure;
using LaborStats.Infrastructure.Data.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Missing required connection string: ConnectionStrings:DefaultConnection");

builder.Services.AddInfrastructure(connectionString, builder.Configuration);
builder.Services.AddEmailServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddScoped<LaborStats.Application.Abstractions.IImportService, LaborStats.Infrastructure.Services.ImportService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddValidatorsFromAssemblyContaining<CreateRoleRequestValidator>();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();


builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<JwtBearerSchemeTransformer>();
});


var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"]
    ?? throw new InvalidOperationException("Missing required configuration: Jwt:Key");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSection["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

builder.Services
    .AddHealthChecks()
    .AddNpgSql(connectionString);
var corsSection = builder.Configuration.GetSection("Cors");
var allowedOrigins = corsSection.GetSection("AllowedOrigins").Get<string[]>()
    ?? throw new InvalidOperationException(
        "Missing required configuration: Cors:AllowedOrigins");

if (allowedOrigins.Length == 0)
{
    throw new InvalidOperationException(
        "At least one allowed origin must be configured under Cors:AllowedOrigins");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("LaborStatsCors", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


await using (var scope = app.Services.CreateAsyncScope())
{
    var adminSeeder =
        scope.ServiceProvider.GetRequiredService<AdminSeeder>();

    await adminSeeder.SeedAsync(app.Lifetime.ApplicationStopping);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("LaborStatsCors");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();  
