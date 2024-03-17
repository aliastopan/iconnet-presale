using IConnet.Presale.Application.RepresentativeOffices.Queries;

namespace IConnet.Presale.Api.Endpoints.Common;

public class RepresentativeOfficeEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(UriEndpoint.RepresentativeOffice.GetRepresentativeOffices, GetChatRepresentativeOffices).AllowAnonymous();
    }

    internal async Task<IResult> GetChatRepresentativeOffices([FromServices] ISender sender,
        HttpContext httpContext)
    {
        var query = new GetRepresentativeOfficesQuery();
        var result = await sender.Send(query);

        return result.Match(
            value =>
            {
                return Results.Ok(value);
            },
            fault => fault.AsProblem(new ProblemDetails
            {
                Title = "Representative Offices Query Failed",
                Instance = httpContext.Request.Path
            },
            context: httpContext));;
    }
}
