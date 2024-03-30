using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Managers;

public class RootCauseManager
{
    private readonly IRootCauseHttpClient _rootCauseHttpClient;
    private readonly OptionService _optionService;

    public RootCauseManager(IRootCauseHttpClient rootCauseHttpClient,
        OptionService optionService)
    {
        _rootCauseHttpClient = rootCauseHttpClient;
        _optionService = optionService;
    }

    public async Task SetRootCausesAsync()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        try
        {
            var httpResult = await _rootCauseHttpClient.GetRootCausesAsync();

            if (httpResult.IsSuccessStatusCode)
            {
                var response = JsonSerializer.Deserialize<GetRootCausesQueryResponse>(httpResult.Content, options);
                ICollection<RootCausesDto> rootCauseDtos = response!.RootCausesDtos;

                _optionService.PopulateRootCause(rootCauseDtos);
            }
            else
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, options);
                var extension = problemDetails.GetProblemDetailsExtension();
                Log.Warning("Error {message}: ", extension.Errors.First().Message);
            }
        }
        catch (Exception exception)
        {
            Log.Fatal("Fatal error occurred: {message}", exception.Message);
            Environment.Exit(1);
        }
    }
}
