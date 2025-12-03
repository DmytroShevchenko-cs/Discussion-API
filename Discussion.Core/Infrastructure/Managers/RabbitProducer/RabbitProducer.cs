namespace Discussion.Core.Infrastructure.Managers.RabbitProducer;

using MassTransit;

public class RabbitProducer(
    IPublishEndpoint publishEndpoint)
    : IRabbitProducer
{

    public async Task PublishAsync<T>(T payload, CancellationToken cancellationToken = default)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(payload);

        await publishEndpoint.Publish(payload, cancellationToken).ConfigureAwait(false);
    }
}