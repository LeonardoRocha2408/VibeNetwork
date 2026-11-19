using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Server.Domain.Entities;
using SocialNetwork.Server.Extensions;
using SocialNetwork.Server.Infrastructure.Services;
using SocialNetwork.Server.Middlewares;

Env.Load();
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
Console.WriteLine($"Env file: {envPath}");
Console.WriteLine($"Env file exists: {File.Exists(envPath)}");

var builder = WebApplication.CreateBuilder(args);

// Configure MySQL database
builder.Services.DatabaseConfig();
builder.Services.AddHttpContextAccessor();

builder.Services.CorsConfig();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

// Configure services used in VibeNetwork
builder.Services
    .AddJwtConfig()
    .AddServicesConfig()
    .AmazonSqsConfig()
    .AddNeoDatabaseConfig()
    .AddSwaggerGenConfig();

// Configure MediatR
builder.Services.MediatrConfig();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("FrontendOnly");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Ok(new
{
    name = "VibeNetwork",
    status = "Healthy",
    timestamp = DateTimeOffset.UtcNow
}))
.AllowAnonymous()
.ExcludeFromDescription();

app.Run();
