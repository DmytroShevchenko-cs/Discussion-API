namespace Discussion.Web.Extensions;

using Core.Infrastructure.Constants.Swagger;
using Microsoft.OpenApi;
using NodaTime;

public static class SwaggerServiceExtensions
{
    public static void RegisterSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(SwaggerConsts.ApiDocName, new OpenApiInfo
            {
                Version = SwaggerConsts.ApiDocName,
                Title = "Discussion API",
                Description = "App API",
            });

            options.MapType<LocalDate>(() => new OpenApiSchema { Type = JsonSchemaType.String, Format = "date" });
            options.MapType<LocalTime>(() => new OpenApiSchema { Type = JsonSchemaType.String, Format = "time" });

            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Discussion.Web.xml"));
            options.EnableAnnotations();
        });
    }
}