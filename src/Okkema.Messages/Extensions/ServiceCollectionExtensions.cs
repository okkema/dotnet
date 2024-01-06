using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Okkema.Queue.Extensions;
using Okkema.Messages;
using Okkema.Messages.Handlers;
namespace Okkema.Messages.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageHandler<TMessage, THandler>(this IServiceCollection services)
        where TMessage : MessageBase
        where THandler : MessageHandlerBase<TMessage>
    {
        services.AddHostedService<THandler>();
        services.AddSingleton<IMessageHandler<TMessage>, THandler>();
        services.AddChannelConsumer<TMessage>();
        return services;
    }
}