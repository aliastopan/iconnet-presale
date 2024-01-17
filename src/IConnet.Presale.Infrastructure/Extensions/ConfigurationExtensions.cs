using Microsoft.Extensions.Configuration;

namespace IConnet.Presale.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static bool UseInMemoryDatabase(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("UseInMemoryDatabase");
    }
}
