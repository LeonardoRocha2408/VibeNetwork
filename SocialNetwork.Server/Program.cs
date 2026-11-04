using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Server.Domain.Entities;
using SocialNetwork.Server.Extensions;
using SocialNetwork.Server.Infrastructure.Services;
using SocialNetwork.Server.Middlewares;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Configure MySQL database
builder.Services.DatabaseConfig();
builder.Services.AddHttpContextAccessor();

builder.Services.CorsConfig();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

// Configure "PasswordHashService" as service 
builder.Services.AddScoped<PasswordHashService>();
builder.Services.AddSingleton<PasswordHasher<UserScaffold>>();

// Configure MediatR
builder.Services.MediatrConfig();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
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
