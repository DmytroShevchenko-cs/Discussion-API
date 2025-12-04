namespace Discussion.Web.Extensions;

using Amazon.S3;
using Core.Database;
using Core.Infrastructure.Configurations;
using Core.Infrastructure.Constants.Rabbit;
using Core.Infrastructure.Managers.RabbitConsumer;
using Core.Infrastructure.Managers.RabbitProducer;
using Core.Infrastructure.Processing.RabbitMq.Comment;
using Core.Services.CommentService;
using Core.Services.R2StorageService;
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

        services.AddTransient<ICommentService, CommentService>();
        services.AddTransient<IR2StorageService, R2StorageService>();
        services.AddSingleton<IAmazonS3, AmazonS3Client>();
        
        services.AddTransient<NewCommentMessageProcessing>();

        return services;
    }

    public static IServiceCollection RegisterRabbitMq(this IServiceCollection services)
    {
        services.AddTransient<IRabbitProducer, RabbitProducer>();

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

        return services;
    }

    public static IServiceCollection RegisterAWSClient(this IServiceCollection services)
    { 
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<AWSR2Settings>>().Value;

            var endpoint = $"https://{options.AccountId}.r2.cloudflarestorage.com";

            var s3Config = new AmazonS3Config
            {
                ServiceURL = endpoint,
                ForcePathStyle = true, 
                UseHttp = false,
            };

            return new AmazonS3Client(
                options.AccessKey,
                options.SecretKey,
                s3Config
            );
        });
        
        return services;
    }
}