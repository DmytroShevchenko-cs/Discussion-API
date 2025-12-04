namespace Discussion.Core.Infrastructure.Managers.RabbitConsumer;

using System.Threading.Tasks;
using MassTransit;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Processing.RabbitMq.Comment;

public class CommentConsumer : IConsumer<CommentMessageDTO>
{
    private readonly ILogger<CommentConsumer> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public CommentConsumer(ILogger<CommentConsumer> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Consume(ConsumeContext<CommentMessageDTO> context)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var processor = scope.ServiceProvider.GetRequiredService<NewCommentMessageProcessing>();
            
            await processor.Invoke(context.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing comment message");
        }
    }
}
