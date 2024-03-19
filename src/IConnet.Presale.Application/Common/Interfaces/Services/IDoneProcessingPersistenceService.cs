namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IDoneProcessingPersistenceService
{
    Task ArchiveValueAsync(string key, string value, TimeSpan? expiry = null);
}
