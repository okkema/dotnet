using System.Text.Json;
using Microsoft.Extensions.Options;
using MQTTnet;
using Okkema.Queue.Extensions;
using Okkema.Queue.Options;

namespace Okkema.Queue.Producers;
public class MqttProducer<T> : IProducer<T>
{
    private readonly IOptionsMonitor<MqttOptions> _options;
    public MqttProducer(IOptionsMonitor<MqttOptions> options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }
    public async Task WriteAsync(T value)
    {
        var mqttFactory = new MqttClientFactory();
        using var mqttClient = mqttFactory.CreateMqttClient();
        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(_options.CurrentValue.Host)
            .Build();
        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(_options.CurrentValue.GetMessageTopic<T>())
            .WithPayload(JsonSerializer.Serialize(value))
            .Build();
        await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
        await mqttClient.DisconnectAsync();
    }
}