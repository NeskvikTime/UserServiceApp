using Serilog;
using UserServiceApp.API;
using UserServiceApp.Application;
using UserServiceApp.Infrastructure;
using UserServiceApp.Infrastructure.DbInitializer;

var builder = WebApplication.CreateBuilder(args);
{
    // Serilog configuration
    builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
        .WriteTo.Console()
        .ReadFrom.Configuration(context.Configuration));


    builder.Services.RegisterApiServices()
        .RegisterApplicationServices()
        .RegisterInfrastructureServices(builder.Configuration);
}

var app = builder.Build();
{
    app.UseExceptionHandler();
    app.AddApiMiddleware();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();

    app.ApplyMigrations();

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}