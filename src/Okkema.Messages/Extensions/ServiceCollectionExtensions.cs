using Microsoft.Extensions.DependencyInjection;
using Okkema.Queue.Extensions;
using Okkema.Messages.Handlers;
using Microsoft.Extensions.Configuration;
namespace Okkema.Messages.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageHandler<TMessage, THandler>(this IServiceCollection services)
        where TMessage : MessageBase
        where THandler : MessageHandlerBase<TMessage>
    {
        services.AddHostedService<THandler>();
        services.AddSingleton<IMessageHandler<TMessage>, THandler>();
        return services;
    }
    public static IServiceCollection AddChannelMessageHandler<TMessage, THandler>(this IServiceCollection services)
        where TMessage : MessageBase
        where THandler : MessageHandlerBase<TMessage>
    {
        services.AddMessageHandler<TMessage, THandler>();
        services.AddChannelConsumer<TMessage>();
        return services;
    }
    public static IServiceCollection AddMqttMessageHandler<TMessage, THandler>(this IServiceCollection services, IConfiguration configuration)
        where TMessage : MessageBase
        where THandler : MessageHandlerBase<TMessage>
    {
        services.AddMessageHandler<TMessage, THandler>();
        services.AddMqttConsumer<TMessage>(configuration);
        return services;
    }

}