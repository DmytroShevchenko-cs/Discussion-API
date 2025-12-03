namespace Discussion.Web.Extensions;

using Configurations;
using Core.Database;
using Core.Infrastructure.Constants.Rabbit;
using Core.Infrastructure.Managers.RabbitConsumer;
using Core.Infrastructure.Managers.RabbitProducer;
using MassTransit;
using Microsoft.Extensions.Options;

public static class CustomServiceExtensions
{
    public static IServiceCollection RegisterCustomServices(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(BaseDbContext).Assembly);
        });
        
        return services;
    }

    public static IServiceCollection RegisterRabbitMq(this IServiceCollection services)
    {
        services.AddTransient<IRabbitProducer, RabbitProducer>();

        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer<CommentConsumer>();

            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<CommentConsumer>();

                cfg.UsingRabbitMq((context, bus) =>
                {
                    var settings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

                    bus.Host(settings.HostName, settings.Port, "/", h =>
                    {
                        h.Username(settings.UserName);
                        h.Password(settings.Password);
                    });

                    bus.ReceiveEndpoint(RabbitConsts.MessageQueue.Comment, e =>
                    {
                        e.ConfigureConsumer<CommentConsumer>(context);
                    });
                });
            });
        });

        return services;
    }
}