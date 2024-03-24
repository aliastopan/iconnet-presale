using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Services;

public class StartupService : IHostedService
{
    private readonly IWorkloadSynchronizationManager _workloadSynchronizationManager;
    private readonly IRepresentativeOfficeHttpClient _representativeOfficeHttpClient;
    private readonly IRootCauseHttpClient _rootCauseHttpClient;
    private readonly UserManager _userManager;
    private readonly ChatTemplateManager _chatTemplateManager;
    private readonly OptionService _optionService;

    public StartupService(IWorkloadSynchronizationManager workloadSynchronizationManager,
        IRepresentativeOfficeHttpClient representativeOfficeHttpClient,
        IRootCauseHttpClient rootCauseHttpClient,
        UserManager userManager,
        ChatTemplateManager chatTemplateManager,
        OptionService optionService)
    {
        _workloadSynchronizationManager = workloadSynchronizationManager;
        _representativeOfficeHttpClient = representativeOfficeHttpClient;
        _rootCauseHttpClient = rootCauseHttpClient;
        _userManager = userManager;
        _chatTemplateManager = chatTemplateManager;
        _optionService = optionService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Task[] tasks =
        [
            GetRepresentativeOfficesAsync(),
            _chatTemplateManager.SetDefaultChatTemplatesAsync(),
            _userManager.SetUserOperatorsAsync(),
            GetRootCausesAsync(),
            PullRedisToInMemoryAsync()
        ];

        await Task.WhenAll(tasks);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Log.Information("Closing application");

        return Task.CompletedTask;
    }

    private async Task GetRepresentativeOfficesAsync()
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

    private async Task GetRootCausesAsync()
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

    private async Task PullRedisToInMemoryAsync()
    {
        await _workloadSynchronizationManager.PullRedisToInMemoryAsync();
    }
}
