namespace Discussion.Web;

using Core.Infrastructure.Hubs;
using Extensions;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = CreateApplicationBuilder(args);
        
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(8080);
        });
        
        builder.Services
            .RegisterConfigurations(builder.Configuration)
            .RegisterInfrastructureServices()
            .RegisterDatabaseAccess(builder.Configuration)
            .RegisterCustomServices()
            .RegisterRabbitMq()
            .RegisterAWSClient()
            .RegisterSwagger(builder.Configuration);
        
        var app = builder.Build();
        
        var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
        
        app.Urls.Add($"http://0.0.0.0:{port}");
        
        app.UseForwardedHeaders();
        app.UseExceptionHandler(_ => { });
        app.UseRouting();
        
        app.UseConfiguredSwagger();
        
        app.MapControllers();
        
        app.MapHub<CommentsHub>("/comments");
        
        await app.ExecuteStartupActions();
        await app.RunAsync();
    }
    
    private static WebApplicationBuilder CreateApplicationBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        return builder;
    }
}