using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Managers;

public class DirectApprovalManager
{
    private readonly IDirectApprovalHttpClient _directApprovalHttpClient;
    private readonly OptionService _optionService;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public DirectApprovalManager(IDirectApprovalHttpClient directApprovalHttpClient,
        OptionService optionService)
    {
        _directApprovalHttpClient = directApprovalHttpClient;
        _optionService = optionService;

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task SetDirectApprovalsAsync()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        try
        {
            var httpResult = await _directApprovalHttpClient.GetDirectApprovalsAsync();

            if (httpResult.IsSuccessStatusCode)
            {
                var response = JsonSerializer.Deserialize<GetDirectApprovalsQueryResponse>(httpResult.Content, options);
                ICollection<DirectApprovalDto> directApprovalDtos = response!.DirectApprovalDto;

                _optionService.PopulateDirectApproval(directApprovalDtos);
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

    public async Task<IQueryable<DirectApprovalSettingModel>> GetDirectApprovalSettingModelsAsync()
    {
        List<DirectApprovalSettingModel> directApprovalSettingModels = [];

        try
        {
            var httpResult = await _directApprovalHttpClient.GetDirectApprovalsAsync();

            if (httpResult.IsSuccessStatusCode)
            {
                var response = JsonSerializer.Deserialize<GetDirectApprovalsQueryResponse>(httpResult.Content, _jsonSerializerOptions);
                ICollection<DirectApprovalDto> directApprovalDtos = response!.DirectApprovalDto;

                foreach (var dto in directApprovalDtos)
                {
                    directApprovalSettingModels.Add(new DirectApprovalSettingModel(dto));
                }
            }
            else
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, _jsonSerializerOptions);
                var extension = problemDetails.GetProblemDetailsExtension();
                Log.Warning("Error {message}: ", extension.Errors.First().Message);
            }

            return directApprovalSettingModels.AsQueryable();
        }
        catch (Exception exception)
        {
            Log.Fatal("Fatal error occurred: {message}", exception.Message);

            return directApprovalSettingModels.AsQueryable();
        }
    }

    public async Task<bool> AddDirectApprovalAsync(int order, string description)
    {
        try
        {
            var httpResult = await _directApprovalHttpClient.AddDirectApprovalAsync(order, description);

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

    public async Task<bool> ToggleSoftDeletionAsync(Guid directApprovalId, bool isDeleted)
    {
        try
        {
            var httpResult = await _directApprovalHttpClient.ToggleSoftDeletionAsync(directApprovalId, isDeleted);

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
