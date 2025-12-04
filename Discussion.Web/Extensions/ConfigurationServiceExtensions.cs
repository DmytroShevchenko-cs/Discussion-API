namespace Discussion.Web.Extensions;

using Core.Infrastructure.Configurations;

public static class ConfigurationServiceExtensions
{
    public static IServiceCollection RegisterConfigurations(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection(nameof(RabbitMqSettings)));
        services.Configure<R2Settings>(configuration.GetSection(nameof(R2Settings)));

        return services;
    }
}