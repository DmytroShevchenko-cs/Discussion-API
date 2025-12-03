namespace Discussion.Web;

using Extensions;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = CreateApplicationBuilder(args);
        
        builder.Services
            .RegisterConfigurations(builder.Configuration)
            .RegisterInfrastructureServices()
            .RegisterDatabaseAccess(builder.Configuration)
            .RegisterCustomServices()
            .RegisterRabbitMq()
            .RegisterSwagger(builder.Configuration);
        
        var app = builder.Build();
        
        app.UseForwardedHeaders();
        app.UseExceptionHandler(_ => { });
        app.UseRouting();
        
        app.UseConfiguredSwagger();
        
        app.MapControllers();
        
        await app.ExecuteStartupActions();
        await app.RunAsync();
    }
    
    private static WebApplicationBuilder CreateApplicationBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        return builder;
    }
}