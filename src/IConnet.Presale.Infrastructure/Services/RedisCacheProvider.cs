using StackExchange.Redis;

namespace IConnet.Presale.Infrastructure.Services;

internal sealed class RedisCacheProvider : ICacheService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisCacheProvider(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public IDatabase Redis => _connectionMultiplexer.GetDatabase(3);

    public async Task<string?> GetCacheValueAsync(string key)
    {
        return await Redis.StringGetAsync(key);
    }

    public async Task SetCacheValueAsync(string key, string value, TimeSpan? expiry = null)
    {
        await Redis.StringSetAsync(key, value, expiry);
    }

    public async Task<bool> IsKeyExistsAsync(string key)
    {
        return await Redis.KeyExistsAsync(key);
    }

    public async Task<bool> DeleteCacheValueAsync(string key)
    {
        return await Redis.KeyDeleteAsync(key);
    }
}
