namespace Discussion.Core.Infrastructure.Managers.RabbitConsumer;

using System.Threading.Tasks;
using MassTransit;
using Messages;
using Microsoft.Extensions.Logging;

public class CommentConsumer : IConsumer<CommentMessage>
{
    private readonly ILogger<CommentConsumer> _logger;

    public CommentConsumer(ILogger<CommentConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<CommentMessage> context)
    {
        _logger.LogInformation("Received comment: {CommentId}", context.Message.CommentId);
        return Task.CompletedTask;
    }
}
