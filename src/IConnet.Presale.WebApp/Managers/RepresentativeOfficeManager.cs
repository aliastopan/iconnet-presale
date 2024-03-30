using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Managers;

public class RepresentativeOfficeManager
{
    private readonly IRepresentativeOfficeHttpClient _representativeOfficeHttpClient;
    private readonly OptionService _optionService;

    public RepresentativeOfficeManager(IRepresentativeOfficeHttpClient representativeOfficeHttpClient,
        OptionService optionService)
    {
        _representativeOfficeHttpClient = representativeOfficeHttpClient;
        _optionService = optionService;
    }

    public async Task SetRepresentativeOfficesAsync()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        try
        {
            var httpResult = await _representativeOfficeHttpClient.GetRepresentativeOfficesAsync();

            if (httpResult.IsSuccessStatusCode)
            {
                var response = JsonSerializer.Deserialize<GetRepresentativeOfficesQueryResponse>(httpResult.Content, options);
                ICollection<RepresentativeOfficeDto> representativeOfficeDtos = response!.RepresentativeOfficeDtos;

                _optionService.PopulateKantorPerwakilan(representativeOfficeDtos);
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
