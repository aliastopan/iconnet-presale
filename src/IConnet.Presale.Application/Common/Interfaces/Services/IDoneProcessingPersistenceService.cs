namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IDoneProcessingPersistenceService
{
    Task<string?> GetValueAsync(string key);
    Task<string?> GetScoredValueAsync(string key);
    Task<List<string?>> GetAllValuesAsync();
    Task<List<string?>> GetAllScoredValuesAsync(long startUnixTime, long endUnixTime);
    Task ArchiveValueAsync(string key, string value, long timestamp, TimeSpan? expiry = null);
    Task<bool> DeleteValueAsync(string key);
    Task<bool> IsKeyExistsAsync(string key);
    Task<HashSet<string>> GetExistingKeysAsync(HashSet<string> keysToCheck);
}
