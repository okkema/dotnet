using Microsoft.Extensions.Logging;
using Okkema.Messages.Handlers;
using Okkema.Queue.Consumers;
namespace Okkema.Messages.Test;
public class TestMessageHandler : MessageHandlerBase<TestMessage>
{
    public TestMessageHandler(ILogger<TestMessageHandler> logger, IConsumer<TestMessage> consumer) : base(logger, consumer) { }
    public override Task HandleAsync(TestMessage message, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
