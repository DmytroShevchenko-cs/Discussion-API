namespace Discussion.Core.Infrastructure.Managers.RabbitProducer;

public interface IRabbitProducer
{
    Task PublishAsync<T>(T payload, CancellationToken cancellationToken = default)
        where T : class;
}