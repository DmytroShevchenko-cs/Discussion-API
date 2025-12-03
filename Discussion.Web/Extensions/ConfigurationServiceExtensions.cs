namespace Discussion.Web.Extensions;

using Configurations;

public static class ConfigurationServiceExtensions
{
    public static IServiceCollection RegisterConfigurations(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection(nameof(RabbitMqSettings)));

        return services;
    }
}