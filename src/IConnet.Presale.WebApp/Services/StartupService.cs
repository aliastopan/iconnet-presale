using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Services;

public class StartupService : IHostedService
{
    private readonly IRepresentativeOfficeHttpClient _representativeOfficeHttpClient;
    private readonly OptionService _optionService;
    private readonly IChatTemplateHttpClient _chatTemplateHttpClient;
    private readonly ChatTemplateService _chatTemplateService;

    public StartupService(IRepresentativeOfficeHttpClient representativeOfficeHttpClient,
        OptionService optionService,
        IChatTemplateHttpClient chatTemplateHttpClient,
        ChatTemplateService chatTemplateService)
    {
        _representativeOfficeHttpClient = representativeOfficeHttpClient;
        _optionService = optionService;
        _chatTemplateHttpClient = chatTemplateHttpClient;
        _chatTemplateService = chatTemplateService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        LogSwitch.Debug("Starting application");

        await GetChatTemplatesAsync();
        await GetRepresentativeOfficesAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        LogSwitch.Debug("Closing application");

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
                var extension = problemDetails.GetExtension();

                LogSwitch.Debug("Error {message}: ", extension.Errors.First().Message);
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
                var extension = problemDetails.GetExtension();

                LogSwitch.Debug("Error {message}: ", extension.Errors.First().Message);
            }
        }
        catch (Exception exception)
        {
            Log.Fatal("Fatal error occurred: {message}", exception.Message);
            Environment.Exit(1);
        }
    }
}
