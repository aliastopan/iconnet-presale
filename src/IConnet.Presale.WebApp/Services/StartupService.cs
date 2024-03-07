using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Services;

public class StartupService : IHostedService
{
    private readonly IWorkloadManager _workloadManager;
    private readonly IRepresentativeOfficeHttpClient _representativeOfficeHttpClient;
    private readonly IRootCauseHttpClient _rootCauseHttpClient;
    private readonly IChatTemplateHttpClient _chatTemplateHttpClient;
    private readonly ChatTemplateService _chatTemplateService;
    private readonly OptionService _optionService;

    public StartupService(IWorkloadManager workloadManager,
        IRepresentativeOfficeHttpClient representativeOfficeHttpClient,
        IRootCauseHttpClient rootCauseHttpClient,
        IChatTemplateHttpClient chatTemplateHttpClient,
        ChatTemplateService chatTemplateService,
        OptionService optionService)
    {
        _workloadManager = workloadManager;
        _representativeOfficeHttpClient = representativeOfficeHttpClient;
        _rootCauseHttpClient = rootCauseHttpClient;
        _chatTemplateHttpClient = chatTemplateHttpClient;
        _chatTemplateService = chatTemplateService;
        _optionService = optionService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // LogSwitch.Debug("Starting application");

        Task[] tasks =
        [
            GetRepresentativeOfficesAsync(),
            GetChatTemplatesAsync(),
            GetRootCausesAsync(),
            SynchronizeRedisToInMemory()
        ];

        await Task.WhenAll(tasks);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // LogSwitch.Debug("Closing application");

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

                // LogSwitch.Debug("Error {message}: ", extension.Errors.First().Message);
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

                // LogSwitch.Debug("Error {message}: ", extension.Errors.First().Message);
            }
        }
        catch (Exception exception)
        {
            Log.Fatal("Fatal error occurred: {message}", exception.Message);
            Environment.Exit(1);
        }
    }

    private async Task GetChatTemplatesAsync()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        try
        {
            var templateName = "default";
            var httpResult = await _chatTemplateHttpClient.GetChatTemplatesAsync(templateName);

            if (httpResult.IsSuccessStatusCode)
            {
                var response = JsonSerializer.Deserialize<GetChatTemplatesQueryResponse>(httpResult.Content, options);
                ICollection<ChatTemplateDto> chatTemplateDtos = response!.ChatTemplateDtos;

                _chatTemplateService.InitializeChatTemplate(chatTemplateDtos);
            }
            else
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, options);
                var extension = problemDetails.GetProblemDetailsExtension();

                // LogSwitch.Debug("Error {message}: ", extension.Errors.First().Message);
            }
        }
        catch (Exception exception)
        {
            Log.Fatal("Fatal error occurred: {message}", exception.Message);
            Environment.Exit(1);
        }
    }

    private async Task SynchronizeRedisToInMemory()
    {
        await _workloadManager.SynchronizeRedisToInMemoryAsync();
    }
}
