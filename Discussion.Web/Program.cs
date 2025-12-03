namespace Discussion.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = CreateApplicationBuilder(args);
        
        
        var app = builder.Build();
        
        app.UseForwardedHeaders();
        app.UseExceptionHandler(_ => { });
        app.UseRouting();
        
        
        app.MapControllers();
        
        await app.RunAsync();
    }
    
    private static WebApplicationBuilder CreateApplicationBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        return builder;
    }
}