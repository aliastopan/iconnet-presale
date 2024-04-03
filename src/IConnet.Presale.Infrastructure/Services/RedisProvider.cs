using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace IConnet.Presale.Infrastructure.Services;

internal sealed class RedisProvider : IInProgressPersistenceService, IDoneProcessingPersistenceService
{
    private int _inProgressDbIndex = 0;
    private int _archiveDbIndex = 1;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly AppSecretSettings _appSecretSettings;

    public RedisProvider(IConnectionMultiplexer connectionMultiplexer,
        IOptions<AppSecretSettings> appSecretOptions)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _appSecretSettings = appSecretOptions.Value;

        _inProgressDbIndex = _appSecretSettings.RedisDbIndex;
        _archiveDbIndex = _appSecretSettings.RedisDbIndex + 1;
    }

    public IDatabase DatabaseProgress => _connectionMultiplexer.GetDatabase(_inProgressDbIndex);
    public IDatabase DatabaseArchive => _connectionMultiplexer.GetDatabase(_archiveDbIndex);

    async Task<string?> IInProgressPersistenceService.GetValueAsync(string key)
    {
        return await GetValueAsync(key, DatabaseProgress);
    }

    // async Task<string?> IDoneProcessingPersistenceService.GetValueAsync(string key)
    // {
    //     return await GetValueAsync(key, DatabaseArchive);
    // }

    async Task<string?> IDoneProcessingPersistenceService.GetScoredValueAsync(string key)
    {
        try
        {
            Log.Warning("Attempting to get scored value.");

            var values = await DatabaseArchive.SortedSetRangeByScoreAsync(key,
                double.NegativeInfinity,
                double.PositiveInfinity);

            return values.FirstOrDefault().ToString();
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }

    async Task<List<string?>> IInProgressPersistenceService.GetAllValuesAsync()
    {
        return await GetAllValuesAsync(_inProgressDbIndex, DatabaseProgress, batchSize: 50);
    }

    async Task<List<string?>> IDoneProcessingPersistenceService.GetAllValuesAsync()
    {
        return await GetAllValuesAsync(_archiveDbIndex, DatabaseArchive, batchSize: 100);
    }

    public async Task<List<string?>> GetAllScoredValuesAsync(long startUnixTime, long endUnixTime)
    {
        Log.Warning("Fetching all scored values");

        return await GetAllScoredValuesAsync(_archiveDbIndex, DatabaseArchive,
            startUnixTime, endUnixTime, batchSize: 100);
    }

    public async Task SetValueAsync(string key, string value, TimeSpan? expiry = null)
    {
        try
        {
            await DatabaseProgress.StringSetAsync(key, value, expiry);
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
            var exists = await DatabaseProgress.KeyExistsAsync(key);
            if (exists)
            {
                await DatabaseProgress.StringSetAsync(key, value, expiry);
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

    async Task<bool> IInProgressPersistenceService.DeleteValueAsync(string key)
    {
        return await DeleteValueAsync(key, DatabaseProgress);
    }

    async Task<bool> IDoneProcessingPersistenceService.DeleteValueAsync(string key)
    {
        return await DeleteValueAsync(key, DatabaseArchive);
    }

    async Task<bool> IInProgressPersistenceService.IsKeyExistsAsync(string key)
    {
        return await IsKeyExistsAsync(key, DatabaseProgress);
    }

    async Task<bool> IDoneProcessingPersistenceService.IsKeyExistsAsync(string key)
    {
        return await IsKeyExistsAsync(key, DatabaseArchive);
    }

    async Task<HashSet<string>> IInProgressPersistenceService.GetExistingKeysAsync(HashSet<string> keysToCheck)
    {
        return await GetExistingKeysAsync(keysToCheck, DatabaseProgress);
    }

    async Task<HashSet<string>> IDoneProcessingPersistenceService.GetExistingKeysAsync(HashSet<string> keysToCheck)
    {
        return await GetExistingKeysAsync(keysToCheck, DatabaseArchive);
    }

    public async Task ArchiveValueAsync(string key, string value, long timestamp, TimeSpan? expiry = null)
    {
        try
        {
            await DatabaseArchive.SortedSetAddAsync(key, value, timestamp);
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis backup operation timed out: {exception.Message}");
            throw;
        }
    }

    private static async Task<string?> GetValueAsync(string key, IDatabase database)
    {
        try
        {
            return await database.StringGetAsync(key);
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }

    private async Task<List<string?>> GetAllValuesAsync(int dbIndex, IDatabase database, int batchSize = 10)
    {
        try
        {
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
            var keys = server.Keys(dbIndex);
            var values = new List<string?>();

            Log.Warning("Count: {0}", keys.Count());

            int numberOfBatches = (int)Math.Ceiling((double)keys.Count() / batchSize);

            var batchTasks = new List<Task>();

            for (int i = 0; i < numberOfBatches; i++)
            {
                var batchKeys = keys.Skip(i * batchSize).Take(batchSize).ToList();

                var batchTask = Task.Run(async () =>
                {
                    Log.Information("Batch: {0}/{1}", i, numberOfBatches);

                    var tasks = batchKeys.Select(key => database.StringGetAsync(key));
                    var batchValues = await Task.WhenAll(tasks);

                    var stringValues = batchValues.Select(value => value.ToString());

                    lock (values)
                    {
                        values.AddRange(stringValues);
                    }
                });

                batchTasks.Add(batchTask);
            }

            await Task.WhenAll(batchTasks);

            return values;
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }

    private static async Task<bool> DeleteValueAsync(string key, IDatabase database)
    {
        try
        {
            return await database.KeyDeleteAsync(key);
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }

    private async Task<List<string?>> GetAllScoredValuesAsync(int dbIndex, IDatabase database,
        long startUnixTime, long endUnixTime, int batchSize = 10)
    {
        try
        {
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
            var keys = server.Keys(dbIndex);
            var values = new List<string?>();

            int numberOfBatches = (int)Math.Ceiling((double)keys.Count() / batchSize);

            for (int i = 0; i < numberOfBatches; i++)
            {
                Log.Information("Batch: {0}/{1}", i, numberOfBatches - 1);

                var batchKeys = keys.Skip(i * batchSize).Take(batchSize).ToList();
                var tasks = batchKeys.Select(key => database.SortedSetRangeByScoreAsync(key, startUnixTime, endUnixTime));
                var batchValues = await Task.WhenAll(tasks);

                Parallel.ForEach(batchValues, elements =>
                {
                    var stringValues = elements.Select(value => value.ToString());
                    lock (values)
                    {
                        values.AddRange(stringValues);
                    }
                });
            }

            return values;
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
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
