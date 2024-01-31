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
        try
        {
            return await Redis.StringGetAsync(key);
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }

    public async Task<List<string?>> GetAllCacheValuesAsync()
    {
        try
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
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }

    }

    public async Task SetCacheValueAsync(string key, string value, TimeSpan? expiry = null)
    {
        try
        {
            await Redis.StringSetAsync(key, value, expiry);
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }

    public async Task<bool> UpdateCacheValueAsync(string key, string value, TimeSpan? expiry = null)
    {
        try
        {
            var exists = await Redis.KeyExistsAsync(key);
            if (exists)
            {
                await Redis.StringSetAsync(key, value, expiry);
                return true;
            }

            return false;
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteCacheValueAsync(string key)
    {
        try
        {
            return await Redis.KeyDeleteAsync(key);
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }

    public async Task<bool> IsKeyExistsAsync(string key)
    {
        try
        {
            return await Redis.KeyExistsAsync(key);
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }
}
