namespace Discussion.Web.Extensions;

using Core.Database;
using Core.Infrastructure.Constants.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

public static class WebApplicationExtensions
{
    public static async Task ExecuteStartupActions(this WebApplication app)
    {
        var serviceScopeFactory = app.Services.GetService<IServiceScopeFactory>();
        using var scope = serviceScopeFactory!.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();
        await dbContext.Database.MigrateAsync();
    }
    
    public static void UseConfiguredSwagger(this WebApplication app)
    {
        app.UseSwagger(c =>
            {
                c.RouteTemplate = "/openapi/{documentName}.json";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer>
                    {
                        new()
                        {
                            Url = $"https://{httpReq.Host.Value}",
                        },
                    };
                });
            })
            .UseSwaggerUI(options =>
            {
                options.RoutePrefix = "swagger";

                options.SwaggerEndpoint(
                    $"/openapi/{SwaggerConsts.ApiDocName}.json",
                    SwaggerConsts.ApiDocName);

                options.DefaultModelExpandDepth(2);
                options.DisplayRequestDuration();
                options.EnableValidator();
            });

        app.MapScalarApiReference(options =>
        {
            options.AddDocument(SwaggerConsts.ApiDocName);
        });
    }
}