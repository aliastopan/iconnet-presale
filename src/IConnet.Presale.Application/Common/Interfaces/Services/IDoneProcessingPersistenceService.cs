namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IDoneProcessingPersistenceService
{
    Task SetBackupValueAsync(string key, string value, TimeSpan? expiry = null);
}
