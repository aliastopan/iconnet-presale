namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface ICacheService
{
    Task<string?> GetCacheValueAsync(string key);
    Task<List<string?>> GetAllCacheValuesAsync();
    Task SetCacheValueAsync(string key, string value, TimeSpan? expiry = null);
    Task<bool> UpdateCacheValueAsync(string key, string value, TimeSpan? expiry = null);
    Task<bool> DeleteCacheValueAsync(string key);
    Task<bool> IsKeyExistsAsync(string key);
}
