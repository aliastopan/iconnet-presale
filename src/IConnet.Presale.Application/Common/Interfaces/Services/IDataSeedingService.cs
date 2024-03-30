namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IDataSeedingService
{
    Task<int> GenerateSuperUserAsync();
    Task<int> GenerateUsersAsync();
    Task<int> GenerateChatTemplatesAsync();
    Task<int> GenerateDirectApprovalsAsync();
    Task<int> GenerateRepresentativeOfficesAsync();
    Task<int> GenerateRootCausesAsync();
}
