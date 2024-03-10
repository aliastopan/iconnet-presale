namespace IConnet.Presale.Api.Endpoints.Common;

public class TestEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/", () => Result.Ok()).AllowAnonymous();
    }
}