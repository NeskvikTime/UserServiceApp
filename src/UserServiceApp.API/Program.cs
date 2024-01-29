using Serilog;
using UserServiceApp.API;
using UserServiceApp.API.Middleware;
using UserServiceApp.Application;
using UserServiceApp.Infrastructure;
using UserServiceApp.Infrastructure.DbInitializer;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.RegisterApiServices()
        .RegisterApplicationServices()
        .RegisterInfrastructureServices(builder.Configuration);

    // Serilog configuration
    builder.Host.UseSerilog((context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    });
}

var app = builder.Build();
{
    app.UseExceptionHandler();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();
    app.UseMiddleware<LoggingMiddleware>();

    app.ApplyMigrations();

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}