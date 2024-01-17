using IConnet.Presale.Application.Common.Interfaces.Persistence;

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
        var task = scope.ServiceProvider.GetRequiredService<IAppDbContextSeeder>().GenerateUsersAsync();
        task.GetAwaiter().GetResult();
    }
}
