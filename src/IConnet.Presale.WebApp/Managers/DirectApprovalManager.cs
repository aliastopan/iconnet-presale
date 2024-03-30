using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Managers;

public class DirectApprovalManager
{
    private readonly IDirectApprovalHttpClient _directApprovalHttpClient;
    private readonly OptionService _optionService;

    public DirectApprovalManager(IDirectApprovalHttpClient directApprovalHttpClient,
        OptionService optionService)
    {
        _directApprovalHttpClient = directApprovalHttpClient;
        _optionService = optionService;
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
}