using MQTTnet;
using Okkema.Queue.Options;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Okkema.Queue.Consumers;
public class MqttConsumer<T> : IConsumer<T>
{
    private readonly IOptionsMonitor<MqttOptions> _options;
    public MqttConsumer(IOptionsMonitor<MqttOptions> options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }
    public async Task ReadAsync(Func<T, CancellationToken, Task> callback, CancellationToken cancellationToken = default)
    {
        var mqttFactory = new MqttClientFactory();
        using var mqttClient = mqttFactory.CreateMqttClient();
        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(_options.CurrentValue.Host)
            .Build();
        mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            var payload = JsonSerializer.Deserialize<T>(e.ApplicationMessage.ConvertPayloadToString());
            if (payload == null) throw new ArgumentNullException("Unable to deserialize MQTT Application Message");
            await callback(payload, cancellationToken);
        };
        await mqttClient.ConnectAsync(mqttClientOptions, cancellationToken);
        var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder().WithTopicFilter("test").Build();
        await mqttClient.SubscribeAsync(mqttSubscribeOptions, cancellationToken);
        await Task.Delay(Timeout.Infinite, cancellationToken); // Wait until cancelled to shutdown gracefully
    }
}