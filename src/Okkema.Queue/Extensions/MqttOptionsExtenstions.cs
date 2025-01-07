using Okkema.Queue.Options;

namespace Okkema.Queue.Extensions;
public static class MqttOptionsExtensions
{
    public static string GetMessageTopic<T>(this MqttOptions options)
    {
        var message = typeof(T).FullName ?? typeof(T).Name;
        if (!options.Messages.TryGetValue(message, out string? topic))
            throw new ArgumentNullException($"Missing MQTT Message Topic {message}");
        return topic;
    }
}