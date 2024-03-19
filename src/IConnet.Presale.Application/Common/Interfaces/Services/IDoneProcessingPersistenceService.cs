namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IDoneProcessingPersistenceService
{
    Task<string?> GetValueAsync(string key);
    Task ArchiveValueAsync(string key, string value, TimeSpan? expiry = null);
    Task<bool> IsKeyExistsAsync(string key);
    Task<HashSet<string>> GetExistingKeysAsync(HashSet<string> keysToCheck);
}
