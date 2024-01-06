namespace Okkema.Messages.Handlers;
public interface IMessageHandler<T> where T : MessageBase
{
    public Task HandleAsync(T message, CancellationToken cancellationToken = default);
}