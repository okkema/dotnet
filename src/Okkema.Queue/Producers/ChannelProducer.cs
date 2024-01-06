using System.Threading.Channels;
namespace Okkema.Queue.Producers;
public sealed class ChannelProducer<T> : IProducer<T>
{
    private readonly Channel<T> _channel;
    public ChannelProducer(
        Channel<T> channel)
    {
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));
    }
    public async Task WriteAsync(T value)
    {
        await _channel.Writer.WriteAsync(value);
    }
}