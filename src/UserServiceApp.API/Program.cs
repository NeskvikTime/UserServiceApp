using Serilog;
using UserServiceApp.API;
using UserServiceApp.API.Endpoints;
using UserServiceApp.API.Middleware;
using UserServiceApp.Application;
using UserServiceApp.Infrastructure;
using UserServiceApp.Infrastructure.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
    options.ValidateOnBuild = context.HostingEnvironment.IsDevelopment();
});

builder.Services
    .RegisterApiServices()
    .RegisterApplicationServices()
    .RegisterInfrastructureServices(builder.Configuration);

// Serilog configuration
builder.Host.UseSerilog((context, configuration)
    => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "UserServiceApp.Api"));
}

app.UseSerilogRequestLogging();
app.UseMiddleware<LoggingMiddleware>();

app.ApplyMigrations();

app.UseHttpsRedirection();

// Add exception handler and status code pages
app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseAuthorization();

// Map controllers for backward compatibility (optional - can be removed once fully migrated to minimal APIs)
app.MapControllers();

// Map minimal API endpoints
app.MapUserEndpoints();

app.Run();
