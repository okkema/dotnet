using Xunit;
using Okkema.Test;
using Okkema.Queue.Consumers;
using Okkema.Queue.Producers;
using Moq;
using Microsoft.Extensions.Options;
using Okkema.Queue.Options;
using MQTTnet.Server;
using FluentAssertions;
using System.Net.Sockets;
namespace Okkema.Cache.Test;
public class MqttQueueTest : IDisposable
{
    private readonly IProducer<MockData> _producer;
    private readonly IConsumer<MockData> _consumer;
    private readonly MqttServer _mqttServer;
    public MqttQueueTest()
    {
        var mqttServerFactory = new MqttServerFactory();
        var mqttServerOptions = new MqttServerOptionsBuilder().WithDefaultEndpoint().Build();
        _mqttServer = mqttServerFactory.CreateMqttServer(mqttServerOptions);
        var options = Mock.Of<IOptionsMonitor<MqttOptions>>();
        Mock.Get(options).Setup(x => x.CurrentValue)
            .Returns(new MqttOptions { Host = "localhost" });
        _producer = new MqttProducer<MockData>(options);
        _consumer = new MqttConsumer<MockData>(options);
    }
    public void Dispose()
    {
        _mqttServer.Dispose();
    }
    [Theory]
    [AutoMockData]
    public async Task ProducesAndConsumesMessage(MockData value)
    {
        try
        {
            await _mqttServer.StartAsync();
        }
        catch (SocketException exception)
        {
            // MQTT broker already running on host machine and tests will use that instead.
            if (exception.Message != "Address already in use") throw;
        } 
        var callback = Mock.Of<Func<MockData, CancellationToken, Task>>();
        _ = Task.Run(() => _consumer.ReadAsync(callback));
        await Task.Delay(1000);
        await _producer.WriteAsync(value);
        await Task.Delay(1000);
        Mock.Get(callback).Verify(x => x(It.Is<MockData>(x => x.Should().BeEquivalentTo(value, "") != null), It.IsAny<CancellationToken>()), Times.Once);
        if (_mqttServer.IsStarted) 
        {
            await _mqttServer.StopAsync();
        }
    }
}
