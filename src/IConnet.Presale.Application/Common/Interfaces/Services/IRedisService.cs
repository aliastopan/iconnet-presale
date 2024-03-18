namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IRedisService
{
    Task<string?> GetValueAsync(string key);
    Task<List<string?>> GetAllValuesAsync();
    Task SetValueAsync(string key, string value, TimeSpan? expiry = null);
    Task<bool> UpdateValueAsync(string key, string value, TimeSpan? expiry = null);
    Task<bool> DeleteValueAsync(string key);
    Task<bool> IsKeyExistsAsync(string key);
    Task<HashSet<string>> GetExistingKeysAsync(HashSet<string> keysToCheck);

    Task SetBackupValueAsync(string key, string value, TimeSpan? expiry = null);
}
