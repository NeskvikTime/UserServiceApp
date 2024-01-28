using UserServiceApp.API;
using UserServiceApp.Application;
using UserServiceApp.Infrastructure;
using UserServiceApp.Infrastructure.DbInitializer;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.RegisterApiServices()
        .RegisterApplicationServices()
        .RegisterInfrastructureServices(builder.Configuration);
}

var app = builder.Build();
{
    app.UseExceptionHandler();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.ApplyMigrations();

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}