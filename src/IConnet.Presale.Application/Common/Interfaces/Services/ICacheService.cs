namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface ICacheService
{
    Task<string?> GetCacheValueAsync(string key);
    Task SetCacheValueAsync(string key, string value, TimeSpan? expiry = null);
    Task<bool> IsKeyExistsAsync(string key);
    Task<bool> DeleteCacheValueAsync(string key);
}
