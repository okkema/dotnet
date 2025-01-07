using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Okkema.Messages.Extensions;
using Okkema.Queue.Consumers;
using Xunit;
namespace Okkema.Messages.Test;
public class MessageHandlerTest
{
    [Fact]
    public async Task InvokesConsumer()
    {
        var logger = Mock.Of<ILogger<TestMessageHandler>>();
        var consumer = Mock.Of<IConsumer<TestMessage>>();
        var services = new ServiceCollection()
            .AddSingleton(logger)
            .AddSingleton(consumer)
            .AddMessageHandler<TestMessage, TestMessageHandler>()
            .BuildServiceProvider();
        using var scope = services.CreateScope();
        var service = (TestMessageHandler)scope.ServiceProvider.GetRequiredService<IHostedService>();
        await service.StartAsync(CancellationToken.None);
        await Task.Delay(1000);
        await service.StopAsync(CancellationToken.None);
        Mock.Get(consumer).Verify(x => x.ReadAsync(service.HandleAsync, It.IsAny<CancellationToken>()), Times.Once());
    }
}
