using IConnet.Presale.Application.Common.Interfaces.Services;

namespace IConnet.Presale.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void InitializeDbContext(this WebApplication app)
    {
        var configuration = app.Services.GetRequiredService<IConfiguration>();
        if (!configuration.UseInMemoryDatabase())
        {
            return;
        }

        using var scope = app.Services.CreateScope();
        var dataSeedingService = scope.ServiceProvider.GetRequiredService<IDataSeedingService>();

        Task[] seedingTasks =
        [
            // dataSeedingService.GenerateSuperUserAsync()
            // dataSeedingService.GenerateUsersAsync(),
            // dataSeedingService.GenerateChatTemplatesAsync(),
            // dataSeedingService.GenerateDirectApprovalsAsync(),
            // dataSeedingService.GenerateRepresentativeOfficesAsync(),
            // dataSeedingService.GenerateRootCausesAsync()
        ];

        Task.WaitAll(seedingTasks);
    }
}
