using System.Threading.Channels;
namespace Okkema.Queue.Consumers;
public sealed class ChannelConsumer<T> : IConsumer<T>
{
    private readonly Channel<T> _channel;
    public ChannelConsumer(
        Channel<T> channel)
    {
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));
    }
    public async Task ReadAsync(Func<T, CancellationToken, Task> callback, CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested 
            && await _channel.Reader.WaitToReadAsync())
        {
            if (_channel.Reader.TryRead(out var value))
            {
                await callback(value, cancellationToken);
            }
        }
    }
}