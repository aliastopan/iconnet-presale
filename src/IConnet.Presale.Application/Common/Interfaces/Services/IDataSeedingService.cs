namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IDataSeedingService
{
    Task<int> GenerateUsersAsync();
    Task<int> GenerateChatTemplatesAsync();
    Task<int> GenerateRepresentativeOfficesAsync();
}
