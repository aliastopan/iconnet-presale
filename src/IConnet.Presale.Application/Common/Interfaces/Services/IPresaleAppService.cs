namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IPresaleAppService
{
    Task<bool> IsNullSettings();
    Task SetDefaultSettingAsync();
    Task SetSettingValueAsync(string key, string value, TimeSpan? expiry = null);
    Task<string?> GetSettingValueAsync(string key);
}
