using Xunit;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Okkema.Test;
using FluentAssertions;
namespace Okkema.Cache.Test;
public class CacheServiceTest
{
    private readonly IDistributedCache _cache;
    private readonly CacheSignal<MockData> _signal;
    private readonly CacheService<MockData> _service;
    public CacheServiceTest()
    {
        var options = Options.Create(new MemoryDistributedCacheOptions());
        _cache = new MemoryDistributedCache(options);
        _signal = new CacheSignal<MockData>();
        _service = new CacheService<MockData>(_cache, _signal);
    }
    [Theory]
    [AutoMockData]
    public async Task GetsDefaultWhenNoHit(string key)
    {
        var result = await _service.GetAsync(key);
        result.Should().BeNull();
    }
    [Theory]
    [AutoMockData]
    public async Task GetsValueThatHasBeenSet(string key, MockData value)
    {
        await _service.SetAsync(key, value);
        var result = await _service.GetAsync(key);
        result.Should().BeEquivalentTo(value);
    }
}
