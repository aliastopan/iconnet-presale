using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace IConnet.Presale.Infrastructure.Services;

internal sealed class RedisCacheProvider : ICacheService
{
    private int _dbIndex = 0;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly AppSecretSettings _appSecretSettings;

    public RedisCacheProvider(IConnectionMultiplexer connectionMultiplexer,
        IOptions<AppSecretSettings> appSecretOptions)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _appSecretSettings = appSecretOptions.Value;

        _dbIndex = _appSecretSettings.RedisDbIndex;
    }

    public IDatabase Redis => _connectionMultiplexer.GetDatabase(_dbIndex);

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
            var keys = server.Keys(_dbIndex);
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
            // Lua script to check if keys exist
            string luaScript = "return redis.call('EXISTS', KEYS[1]) ==  1";

            var result = await Redis.ScriptEvaluateAsync(luaScript, [(RedisKey)key]);

            return (bool)result;
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }

    public async Task<List<string>> GetExistingKeysAsync(List<string> keysToCheck)
    {
        try
        {
            // Lua script to check if keys exist
            string luaScript = @"
                local result = {}
                for i, key in ipairs(KEYS) do
                    if redis.call('EXISTS', key) ==  1 then
                        table.insert(result, key)
                    end
                end
                return result
            ";

            var redisKeys = keysToCheck.Select(key => (RedisKey)key).ToArray();
            var result = await Redis.ScriptEvaluateAsync(luaScript, redisKeys);

            var existingKeys = result.ToString().Split(',').Select(key => key.Trim()).ToList();

            return existingKeys;
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }
}
