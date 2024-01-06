using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Okkema.Queue.Consumers;
namespace Okkema.Messages.Handlers;
public abstract class MessageHandlerBase<T> : BackgroundService, IMessageHandler<T>
    where T : MessageBase
{
    protected readonly ILogger<MessageHandlerBase<T>> _logger;
    private readonly IConsumer<T> _consumer;
    public MessageHandlerBase(
        ILogger<MessageHandlerBase<T>> logger,
        IConsumer<T> consumer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
    }
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await _consumer.ReadAsync(HandleAsync, cancellationToken);
        }
    }
    public abstract Task HandleAsync(T message, CancellationToken cancellationToken = default);
    
}