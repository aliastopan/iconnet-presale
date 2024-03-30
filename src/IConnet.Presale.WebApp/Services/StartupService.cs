using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Services;

public class StartupService : IHostedService
{
    private readonly IWorkloadSynchronizationManager _workloadSynchronizationManager;
    private readonly IRootCauseHttpClient _rootCauseHttpClient;
    private readonly UserManager _userManager;
    private readonly ChatTemplateManager _chatTemplateManager;
    private readonly RepresentativeOfficeManager _representativeOfficeManager;
    private readonly OptionService _optionService;

    public StartupService(IWorkloadSynchronizationManager workloadSynchronizationManager,
        IRootCauseHttpClient rootCauseHttpClient,
        UserManager userManager,
        ChatTemplateManager chatTemplateManager,
        RepresentativeOfficeManager representativeOfficeManager,
        OptionService optionService)
    {
        _workloadSynchronizationManager = workloadSynchronizationManager;
        _rootCauseHttpClient = rootCauseHttpClient;
        _userManager = userManager;
        _chatTemplateManager = chatTemplateManager;
        _representativeOfficeManager = representativeOfficeManager;
        _optionService = optionService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Task[] tasks =
        [
            _chatTemplateManager.SetDefaultChatTemplatesAsync(),
            _representativeOfficeManager.SetRepresentativeOfficesAsync(),
            _userManager.SetPresaleOperatorsAsync(),
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
