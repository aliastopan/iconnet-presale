using IConnet.Presale.Application.Presales.Commands;
using IConnet.Presale.Shared.Contracts.Presale;
using Mapster;

namespace IConnet.Presale.Api.Endpoints.Presale;

public class InsertWorkPaperEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost(UriEndpoint.Presale.InsertWorkPaper, Insert).AllowAnonymous();
    }

    internal async Task<IResult> Insert([FromServices] ISender sender,
        InsertWorkPaperRequest request, HttpContext httpContext)
    {
        var command = request.Adapt<InsertWorkPaperCommand>();
        var result = await sender.Send(command);

        return result.Match(() => Results.Ok(),
            error => error.AsProblem(new ProblemDetails
            {
                Title = "Failed to Insert WorkPaper",
                Instance = httpContext.Request.Path
            },
            context: httpContext));
    }
}
