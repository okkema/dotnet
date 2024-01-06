using Xunit;
using Okkema.Test;
using System.Threading.Channels;
using Okkema.Queue.Consumers;
using Okkema.Queue.Producers;
using Moq;
namespace Okkema.Cache.Test;
public class ChannelQueueTest
{
    private readonly Channel<MockData> _channel;
    private readonly IProducer<MockData> _producer;
    private readonly IConsumer<MockData> _consumer;
    public ChannelQueueTest()
    {
        _channel = Channel.CreateUnbounded<MockData>();
        _producer = new ChannelProducer<MockData>(_channel);
        _consumer = new ChannelConsumer<MockData>(_channel);
    }
    [Theory]
    [AutoMockData]
    public async Task ProducesAndConsumesMessage(MockData value)
    {
        var callback = Mock.Of<Func<MockData, CancellationToken, Task>>();
        _ = Task.Run(() => _consumer.ReadAsync(callback));
        await _producer.WriteAsync(value);
        await Task.Delay(1000);
        Mock.Get(callback).Verify(x => x(value, It.IsAny<CancellationToken>()), Times.Once);
    }
}
