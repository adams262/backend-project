using LaborStats.Infrastructure;
using LaborStats.Infrastructure.Data.Seed;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Missing required connection string: ConnectionStrings:DefaultConnection");
builder.Services.AddInfrastructure(connectionString, builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
