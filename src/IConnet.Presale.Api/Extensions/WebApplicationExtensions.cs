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
        var generateUserTask = scope.ServiceProvider.GetRequiredService<IDataSeedingService>().GenerateUsersAsync();
        generateUserTask.GetAwaiter().GetResult();

        var generateChatTemplatesTask = scope.ServiceProvider.GetRequiredService<IDataSeedingService>().GenerateChatTemplatesAsync();
        generateChatTemplatesTask.GetAwaiter().GetResult();

        var generateRepresentativeOfficesTask = scope.ServiceProvider.GetRequiredService<IDataSeedingService>().GenerateRepresentativeOfficesAsync();
        generateRepresentativeOfficesTask.GetAwaiter().GetResult();
    }
}
