using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Services;

public class StartupService : IHostedService
{
    private readonly IChatTemplateHttpClientService _chatTemplateHttpClientService;
    private readonly ChatTemplateService _chatTemplateService;

    public StartupService(IChatTemplateHttpClientService chatTemplateHttpClientService,
        ChatTemplateService chatTemplateService)
    {
        _chatTemplateHttpClientService = chatTemplateHttpClientService;
        _chatTemplateService = chatTemplateService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        LogSwitch.Debug("Starting application");
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var templateName = "default";
        var httpResult = await _chatTemplateHttpClientService.GetChatTemplateAsync(templateName);

        if (httpResult.IsSuccessStatusCode)
        {
            var response = JsonSerializer.Deserialize<GetChatTemplateResponse>(httpResult.Content, options);
            ICollection<ChatTemplateDto> chatTemplateDtos = response!.ChatTemplateDtos;

            _chatTemplateService.InitializeChatTemplate(chatTemplateDtos);
        }
        else
        {
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, options);
            var extension = problemDetails.GetExtension();

            LogSwitch.Debug("Error {message}: ", extension.Errors.First().Message);
        }

        await Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        LogSwitch.Debug("Closing application");

        return Task.CompletedTask;
    }
}
