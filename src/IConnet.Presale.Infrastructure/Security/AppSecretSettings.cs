namespace IConnet.Presale.Infrastructure.Security;

public class AppSecretSettings
{
    public static class Section
    {
        public const string MasterKey = "AppSecrets:MasterKey";
        public const string MysqlConnectionString = "AppSecrets:MysqlConnectionString";
        public const string RedisConnectionString = "AppSecrets:RedisConnectionString";
        public const string RedisPassword = "AppSecrets:RedisPassword";
        public const string JwtIssuer = "AppSecrets:JwtIssuer";
        public const string JwtAudience = "AppSecrets:JwtAudience";
    }

    public const string SectionName = "AppSecrets";

    public string MasterKey { get; set; } = "";
    public string MysqlConnectionString { get; set; } = "";
    public string RedisConnectionString { get; set; } = "";
    public string RedisPassword { get; set; } = "";

    public string JwtIssuer { get; init; } = null!;
    public string JwtAudience { get; init; } = null!;
    public TimeSpan JwtLifeTime { get; init; }
    public TimeSpan JwtRefreshLifeTime { get; init; }
}
