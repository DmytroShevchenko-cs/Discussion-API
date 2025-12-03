namespace Discussion.Web.Extensions;

using Core.Database;
using FluentValidation;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection RegisterInfrastructureServices(
        this IServiceCollection services)
    {
        services.AddOptions();
        services.AddSignalR();
        services.AddMemoryCache();
        services.AddHttpClient();

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                options.JsonSerializerOptions.Converters.Add(NodaConverters.IntervalConverter);
                options.JsonSerializerOptions.Converters.Add(NodaConverters.InstantConverter);
                options.JsonSerializerOptions.Converters.Add(NodaConverters.LocalDateConverter);
                options.JsonSerializerOptions.Converters.Add(NodaConverters.LocalTimeConverter);
            });

        services.AddValidatorsFromAssemblyContaining<BaseDbContext>();

        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        
        return services;
    }
}