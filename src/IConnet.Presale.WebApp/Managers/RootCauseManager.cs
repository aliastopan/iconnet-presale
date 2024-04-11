using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Managers;

public class RootCauseManager
{
    private readonly IRootCauseHttpClient _rootCauseHttpClient;
    private readonly OptionService _optionService;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public RootCauseManager(IRootCauseHttpClient rootCauseHttpClient,
        OptionService optionService)
    {
        _rootCauseHttpClient = rootCauseHttpClient;
        _optionService = optionService;

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task SetRootCausesAsync()
    {
        try
        {
            var httpResult = await _rootCauseHttpClient.GetRootCausesAsync();

            if (httpResult.IsSuccessStatusCode)
            {
                var response = JsonSerializer.Deserialize<GetRootCausesQueryResponse>(httpResult.Content, _jsonSerializerOptions);
                ICollection<RootCausesDto> rootCauseDtos = response!.RootCausesDtos;

                _optionService.PopulateRootCauseOptions(rootCauseDtos);
            }
            else
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, _jsonSerializerOptions);
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

    public async Task<IQueryable<RootCauseSettingModel>> GetRootCauseSettingModelsAsync()
    {
        List<RootCauseSettingModel> rootCauseSettingModels = [];

        try
        {
            var httpResult = await _rootCauseHttpClient.GetRootCausesAsync();

            if (httpResult.IsSuccessStatusCode)
            {
                var response = JsonSerializer.Deserialize<GetRootCausesQueryResponse>(httpResult.Content, _jsonSerializerOptions);
                ICollection<RootCausesDto> rootCauseDtos = response!.RootCausesDtos;

                foreach (var dto in rootCauseDtos)
                {
                    rootCauseSettingModels.Add(new RootCauseSettingModel(dto));
                }
            }
            else
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, _jsonSerializerOptions);
                var extension = problemDetails.GetProblemDetailsExtension();
                Log.Warning("Error {message}: ", extension.Errors.First().Message);
            }

            return rootCauseSettingModels.AsQueryable();
        }
        catch (Exception exception)
        {
            Log.Fatal("Fatal error occurred: {message}", exception.Message);

            return rootCauseSettingModels.AsQueryable();
        }
    }

    public async Task<bool> AddRootCauseAsync(int order, string cause)
    {
        try
        {
            var httpResult = await _rootCauseHttpClient.AddRootCauseAsync(order, cause);

            if (httpResult.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, _jsonSerializerOptions);
                var extension = problemDetails.GetProblemDetailsExtension();

                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Fatal("Fatal error occurred: {message}", exception.Message);

            return false;
        }
    }

    public async Task<bool> ToggleSoftDeletionAsync(Guid rootCauseId, bool isDeleted)
    {
        try
        {
            var httpResult = await _rootCauseHttpClient.ToggleSoftDeletionAsync(rootCauseId, isDeleted);

            if (httpResult.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, _jsonSerializerOptions);
                var extension = problemDetails.GetProblemDetailsExtension();

                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Fatal("Fatal error occurred: {message}", exception.Message);

            return false;
        }
    }
}
