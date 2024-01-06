namespace Okkema.Queue.Consumers;
public interface IConsumer<T>
{
    public Task ReadAsync(Func<T, CancellationToken, Task> callback, CancellationToken cancellationToken = default);
}