namespace IConnet.Presale.Api.Endpoints.Common;

public class TestEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/", () => Result.Ok()).AllowAnonymous();
        app.MapGet("/appsecrets", GetAppSecrets).AllowAnonymous();
    }

    internal IResult GetAppSecrets(IConfiguration configuration)
    {
        var appSecrets = new
        {
            MasterKey = configuration.GetValue<string>("AppSecrets:MasterKey"),
            MySqlConnectionString = configuration.GetValue<string>("AppSecrets:MySqlConnectionString"),
            RedisConnectionString = configuration.GetValue<string>("AppSecrets:RedisConnectionString"),
            RedisPassword = configuration.GetValue<string>("AppSecrets:RedisPassword"),
            RedisDbIndex = configuration.GetValue<string>("AppSecrets:RedisDbIndex"),
            JwtIssuer = configuration.GetValue<string>("AppSecrets:JwtIssuer"),
            JwtAudience = configuration.GetValue<string>("AppSecrets:JwtAudience"),
            JwtLifeTime = configuration.GetValue<string>("AppSecrets:JwtLifeTime"),
            JwtRefreshLifeTime = configuration.GetValue<string>("AppSecrets:JwtRefreshLifeTime")
        };

        return Results.Ok(appSecrets);
    }
}