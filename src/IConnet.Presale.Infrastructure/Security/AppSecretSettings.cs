namespace IConnet.Presale.Infrastructure.Security;

public class AppSecretSettings
{
    public static class Section
    {
        public const string MasterKey = "AppSecrets:MasterKey";
        public const string MysqlConnectionString = "AppSecrets:MysqlConnectionString";
        public const string RedisConnectionString = "AppSecrets:RedisConnectionString";
        public const string RedisPassword = "AppSecrets:RedisPassword";
        public const string RedisDbIndex = "AppSecrets:RedisDbIndex";
        public const string JwtIssuer = "AppSecrets:JwtIssuer";
        public const string JwtAudience = "AppSecrets:JwtAudience";
    }

    public const string SectionName = "AppSecrets";

    public string MasterKey { get; set; } = string.Empty;
    public string MysqlConnectionString { get; set; } = string.Empty;
    public string RedisConnectionString { get; set; } = string.Empty;
    public string RedisPassword { get; set; } = string.Empty;
    public int RedisDbIndex { get; set; } = 0;

    public string JwtIssuer { get; init; } = string.Empty;
    public string JwtAudience { get; init; } = string.Empty;
    public TimeSpan JwtLifeTime { get; init; } = default;
    public TimeSpan JwtRefreshLifeTime { get; init; } = default;
}
