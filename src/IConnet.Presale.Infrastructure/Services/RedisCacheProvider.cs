using StackExchange.Redis;

namespace IConnet.Presale.Infrastructure.Services;

internal sealed class RedisCacheProvider : ICacheService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisCacheProvider(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<string?> GetCacheValueAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        return await db.StringGetAsync(key);
    }

    public async Task SetCacheValueAsync(string key, string value, TimeSpan? expiry = null)
    {
        var db = _connectionMultiplexer.GetDatabase();
        await db.StringSetAsync(key, value, expiry);
    }

    public async Task<bool> IsKeyExistsAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        return await db.KeyExistsAsync(key);
    }

    public async Task<bool> DeleteCacheValueAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        return await db.KeyDeleteAsync(key);
    }
}
