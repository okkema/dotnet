namespace Okkema.Cache;
public interface ICacheService<T>
{
    public Task<T?> GetAsync(string key, CancellationToken token);
    public Task SetAsync(string key, T value, CancellationToken token);
}
