namespace Discussion.Web.Extensions;

using Core.Database;
using Microsoft.EntityFrameworkCore;
using Npgsql;

public static class DatabaseServiceExtensions
{
    public static IServiceCollection RegisterDatabaseAccess(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<BaseDbContext>(o => o.UseNpgsql(
            connectionString!,
            b =>
            {
                b.MigrationsAssembly("Discussion.Web");
                b.UseNodaTime();
            })
        );

        services.AddSingleton(_ =>
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            dataSourceBuilder.UseNodaTime();
            var dataSource = dataSourceBuilder.Build();

            return dataSource;
        });

        return services;
    }
}