using StackExchange.Redis;

namespace IConnet.Presale.Infrastructure.Services;

internal sealed class RedisCacheProvider : ICacheService
{
    private const int DbIndex = 3;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisCacheProvider(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public IDatabase Redis => _connectionMultiplexer.GetDatabase(DbIndex);

    public async Task<string?> GetCacheValueAsync(string key)
    {
        return await Redis.StringGetAsync(key);
    }

    public async Task<List<string?>> GetAllCacheValuesAsync()
    {
        var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
        var keys = server.Keys(DbIndex);
        var values = new List<string?>();

        foreach (var key in keys)
        {
            var value = await Redis.StringGetAsync(key);
            values.Add(value);
        }

        return values;
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
