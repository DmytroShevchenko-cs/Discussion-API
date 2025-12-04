namespace Discussion.Web.Extensions;

using Core.Database;
using Core.Infrastructure.Constants.Swagger;
using Core.Services.CaptchaService;
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

        services.AddCors(options =>
        {
            options.AddPolicy(SwaggerConsts.CorsPolicy, policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        
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
        
        services.AddSingleton<ICaptchaService, CaptchaService>();
        
        return services;
    }
}