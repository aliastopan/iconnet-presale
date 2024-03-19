using System.Linq;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace IConnet.Presale.Infrastructure.Services;

internal sealed class RedisProvider : IOnProgressPersistenceService, IDoneProcessingPersistenceService
{
    private int _onProgressDbIndex = 0;
    private int _archiveDbIndex = 1;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly AppSecretSettings _appSecretSettings;

    public RedisProvider(IConnectionMultiplexer connectionMultiplexer,
        IOptions<AppSecretSettings> appSecretOptions)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _appSecretSettings = appSecretOptions.Value;

        _onProgressDbIndex = _appSecretSettings.RedisDbIndex;
        _archiveDbIndex = _appSecretSettings.RedisDbIndex + 1;
    }

    public IDatabase RedisOnProgress => _connectionMultiplexer.GetDatabase(_onProgressDbIndex);
    public IDatabase RedisArchive => _connectionMultiplexer.GetDatabase(_archiveDbIndex);

    public async Task<string?> GetValueAsync(string key)
    {
        try
        {
            return await RedisOnProgress.StringGetAsync(key);
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
            var keys = server.Keys(_onProgressDbIndex);
            var values = new List<string?>();

            foreach (var key in keys)
            {
                var value = await RedisOnProgress.StringGetAsync(key);
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
            await RedisOnProgress.StringSetAsync(key, value, expiry);
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
            var exists = await RedisOnProgress.KeyExistsAsync(key);
            if (exists)
            {
                await RedisOnProgress.StringSetAsync(key, value, expiry);
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
            return await RedisOnProgress.KeyDeleteAsync(key);
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }

    async Task<bool> IOnProgressPersistenceService.IsKeyExistsAsync(string key)
    {
        return await IsKeyExistsAsync(key, database: RedisOnProgress);
    }

    async Task<bool> IDoneProcessingPersistenceService.IsKeyExistsAsync(string key)
    {
        return await IsKeyExistsAsync(key, database: RedisArchive);
    }

    async Task<HashSet<string>> IOnProgressPersistenceService.GetExistingKeysAsync(HashSet<string> keysToCheck)
    {
        LogSwitch.Debug("Checking OnProgress Persistence at {0}", _onProgressDbIndex);
        return await GetExistingKeysAsync(keysToCheck, database: RedisOnProgress);
    }

    async Task<HashSet<string>> IDoneProcessingPersistenceService.GetExistingKeysAsync(HashSet<string> keysToCheck)
    {
        LogSwitch.Debug("Checking Archive Persistence at {0}", _archiveDbIndex);
        return await GetExistingKeysAsync(keysToCheck, database: RedisArchive);
    }

    public async Task ArchiveValueAsync(string key, string value, TimeSpan? expiry = null)
    {
        try
        {
            await RedisArchive.StringSetAsync(key, value, expiry);
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis backup operation timed out: {exception.Message}");
            throw;
        }
    }

    private static async Task<bool> IsKeyExistsAsync(string key, IDatabase database)
    {
        try
        {
            // Lua script to check if keys exist
            string luaScript = "return redis.call('EXISTS', KEYS[1]) ==  1";

            var result = await database.ScriptEvaluateAsync(luaScript, new RedisKey[] { key });

            return (bool)result;
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }

    private static async Task<HashSet<string>> GetExistingKeysAsync(HashSet<string> keysToCheck, IDatabase database)
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
            RedisResult[] redisResult = (RedisResult[])(await database.ScriptEvaluateAsync(luaScript, redisKeys))!;

            return redisResult.Select(result => result.ToString()).ToHashSet();
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }
}
