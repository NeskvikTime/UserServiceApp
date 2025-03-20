using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UserServiceApp.API.Swagger;

public static class SwaggerGenExtension
{
    public static SwaggerGenOptions ConfigureSwaggerGenOptions(this SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "UserServiceApp.API",
            Version = "v1",
            Description = "API documentation for UserServiceApp",
        });

        options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme."
        });


        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                },
                new string[] {}
            }
        });

        return options;
    }
}
