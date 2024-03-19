using System.Linq;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace IConnet.Presale.Infrastructure.Services;

internal sealed class RedisProvider : IOnProgressPersistenceService
{
    private int _dbIndex = 0;
    private int _backupDbIndex = 1;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly AppSecretSettings _appSecretSettings;

    public RedisProvider(IConnectionMultiplexer connectionMultiplexer,
        IOptions<AppSecretSettings> appSecretOptions)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _appSecretSettings = appSecretOptions.Value;

        _dbIndex = _appSecretSettings.RedisDbIndex;
        _backupDbIndex = _dbIndex + 1;
    }

    public IDatabase RedisMain => _connectionMultiplexer.GetDatabase(_dbIndex);
    public IDatabase RedisBackup => _connectionMultiplexer.GetDatabase(_backupDbIndex);

    public async Task<string?> GetValueAsync(string key)
    {
        try
        {
            return await RedisMain.StringGetAsync(key);
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }

    public async Task<List<string?>> GetAllValuesAsync()
    {
        try
        {
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
            var keys = server.Keys(_dbIndex);
            var values = new List<string?>();

            foreach (var key in keys)
            {
                var value = await RedisMain.StringGetAsync(key);
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

    public async Task SetValueAsync(string key, string value, TimeSpan? expiry = null)
    {
        try
        {
            await RedisMain.StringSetAsync(key, value, expiry);
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }

    public async Task<bool> UpdateValueAsync(string key, string value, TimeSpan? expiry = null)
    {
        try
        {
            var exists = await RedisMain.KeyExistsAsync(key);
            if (exists)
            {
                await RedisMain.StringSetAsync(key, value, expiry);
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

    public async Task<bool> DeleteValueAsync(string key)
    {
        try
        {
            return await RedisMain.KeyDeleteAsync(key);
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

            var result = await RedisMain.ScriptEvaluateAsync(luaScript, [(RedisKey)key]);

            return (bool)result;
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }

    public async Task<HashSet<string>> GetExistingKeysAsync(HashSet<string> keysToCheck)
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

            RedisKey[]? redisKeys = keysToCheck.Select(key => (RedisKey)key).ToArray();
            RedisResult[] redisResult = (RedisResult[])(await RedisMain.ScriptEvaluateAsync(luaScript, redisKeys))!;

            return redisResult.Select(result => result.ToString()).ToHashSet();
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }

    public async Task SetBackupValueAsync(string key, string value, TimeSpan? expiry = null)
    {
        try
        {
            await RedisBackup.StringSetAsync(key, value, expiry);
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis backup operation timed out: {exception.Message}");
            throw;
        }
    }
}
