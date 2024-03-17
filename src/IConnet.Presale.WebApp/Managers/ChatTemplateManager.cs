using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Managers;

public class ChatTemplateManager
{
    private readonly List<ChatTemplateModel> _chatTemplateModels = new List<ChatTemplateModel>();
    private readonly IChatTemplateHttpClient _chatTemplateHttpClient;

    public ChatTemplateManager(IChatTemplateHttpClient chatTemplateHttpClient)
    {
        _chatTemplateHttpClient = chatTemplateHttpClient;
    }

    public List<ChatTemplateModel> ChatTemplateModels => _chatTemplateModels;

    public async Task SetDefaultChatTemplatesAsync()
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

                SetChatTemplates(chatTemplateDtos);
            }
            else
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, options);
                var extension = problemDetails.GetProblemDetailsExtension();
            }
        }
        catch (Exception exception)
        {
            Log.Fatal("Fatal error occurred: {message}", exception.Message);
            Environment.Exit(1);
        }
    }

    private void SetChatTemplates(ICollection<ChatTemplateDto> chatTemplateDtos)
    {
        foreach (var chatTemplate in chatTemplateDtos)
        {
            _chatTemplateModels.Add(new ChatTemplateModel
            {
                Sequence = chatTemplate.Sequence,
                Content = chatTemplate.Content,
                HtmlContent = (MarkupString)chatTemplate.Content
                    .FormatHtmlBreak()
                    .FormatHtmlBold()
                    .FormatHtmlItalic()
            });
        }
    }
}
