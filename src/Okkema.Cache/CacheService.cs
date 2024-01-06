using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
namespace Okkema.Cache;
public sealed class CacheService<T> : ICacheService<T> where T : class, new()
{
    private readonly IDistributedCache _cache;
    private readonly CacheSignal<T> _signal;
    public CacheService(IDistributedCache cache,
        CacheSignal<T> signal)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _signal = signal ?? throw new ArgumentNullException(nameof(signal));

    }
    public async Task<T?> GetAsync(string key, CancellationToken token = default)
    {
        try
        {
            await _signal.WaitAsync();
            var json = await _cache.GetStringAsync(key, token);
            if (string.IsNullOrWhiteSpace(json)) return default; 
            return JsonSerializer.Deserialize<T>(json);
        }
        finally
        {
            _signal.Release();
        }
    }
    public async Task SetAsync(string key, T value, CancellationToken token = default)
    {
        try
        {
            await _signal.WaitAsync();
            var json = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, json, token);
        }   
        finally
        {
            _signal.Release();
        }
    }
}
