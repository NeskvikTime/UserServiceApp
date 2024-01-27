namespace UserServiceApp.API;
public static class DependencyInjection
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddProblemDetails();
        services.AddHttpContextAccessor();

        return services;
    }
}
