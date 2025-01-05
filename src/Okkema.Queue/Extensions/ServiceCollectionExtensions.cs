using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Okkema.Queue.Consumers;
using Okkema.Queue.Producers;
using System.Threading.Channels;
using Okkema.Queue.Options;
namespace Okkema.Queue.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChannelConsumer<T>(this IServiceCollection services)
        where T : class
    {
        services.AddSingleton<IConsumer<T>, ChannelConsumer<T>>();
        services.TryAddSingleton(Channel.CreateUnbounded<T>());
        return services;
    }
    public static IServiceCollection AddChannelProducer<T>(this IServiceCollection services)
        where T : class
    {
        services.AddSingleton<IProducer<T>, ChannelProducer<T>>();
        services.TryAddSingleton(Channel.CreateUnbounded<T>());
        return services;
    }
    public static IServiceCollection AddMqttConsumer<T>(this IServiceCollection services, IConfiguration configuration)
        where T : class
    {
        services.AddOptions<MqttOptions>()
            .Bind(configuration.GetSection(nameof(MqttOptions)))
            .ValidateDataAnnotations();
        services.AddSingleton<IConsumer<T>, MqttConsumer<T>>();
        return services;
    }
    public static IServiceCollection AddMqttProducer<T>(this IServiceCollection services, IConfiguration configuration)
        where T : class
    {
        services.AddOptions<MqttOptions>()
            .Bind(configuration.GetSection(nameof(MqttOptions)))
            .ValidateDataAnnotations();
        services.AddSingleton<IProducer<T>, MqttProducer<T>>();
        return services;
    }
}