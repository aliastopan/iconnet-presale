namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IDataSeedingService
{
    Task<int> GenerateUsersAsync();
}
