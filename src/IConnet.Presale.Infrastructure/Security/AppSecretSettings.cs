namespace IConnet.Presale.Infrastructure.Security;

public class AppSecretSettings
{
    public static class Section
    {
        public const string MasterKey = "AppSecrets:MasterKey";
        public const string ConnectionString = "AppSecrets:ConnectionString";
        public const string RedisConnection = "AppSecrets:RedisConnection";
        public const string JwtIssuer = "AppSecrets:JwtIssuer";
        public const string JwtAudience = "AppSecrets:JwtAudience";
    }

    public const string SectionName = "AppSecrets";

    public string MasterKey { get; set; } = "";
    public string ConnectionString { get; set; } = "";
    public string RedisConnection { get; set; } = "";

    public string JwtIssuer { get; init; } = null!;
    public string JwtAudience { get; init; } = null!;
    public TimeSpan JwtLifeTime { get; init; }
    public TimeSpan JwtRefreshLifeTime { get; init; }
}
