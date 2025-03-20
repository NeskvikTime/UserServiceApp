using Serilog;
using UserServiceApp.API;
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

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //app.UseSwagger();
    //app.UseSwaggerUI(c =>
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserServiceApp.API v1"));
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "UserServiceApp.Api"));
}

app.UseSerilogRequestLogging();
app.UseMiddleware<LoggingMiddleware>();

app.ApplyMigrations();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
