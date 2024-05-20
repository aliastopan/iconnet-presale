using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Managers;

public class ChatTemplateManager
{
    private readonly List<ChatTemplateModel> _chatTemplateModels = new List<ChatTemplateModel>();
    private readonly IChatTemplateHttpClient _chatTemplateHttpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ChatTemplateManager(IChatTemplateHttpClient chatTemplateHttpClient,
        IConfiguration configuration)
    {
        _chatTemplateHttpClient = chatTemplateHttpClient;

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public List<ChatTemplateModel> ChatTemplateModels => _chatTemplateModels;

    public async Task<bool> ApplyChatTemplateAction(ChatTemplateSettingModel model)
    {
        try
        {
            var httpResult = await _chatTemplateHttpClient.ChatTemplateActionAsync(
                model.ChatTemplateId,
                model.TemplateName,
                model.Sequence,
                model.Content,
                (int)model.ActionSetting);

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

    public async Task SetChatTemplateAsync(string templateName)
    {
        // LogSwitch.Debug("SetChatTemplateAsync {0}", templateName);

        try
        {
            var httpResult = await _chatTemplateHttpClient.GetChatTemplatesAsync(templateName);

            if (httpResult.IsSuccessStatusCode)
            {
                var response = JsonSerializer.Deserialize<GetChatTemplatesQueryResponse>(httpResult.Content, _jsonSerializerOptions);
                ICollection<ChatTemplateDto> chatTemplateDtos = response!.ChatTemplateDtos;

                SetChatTemplates(chatTemplateDtos);
            }
            else
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, _jsonSerializerOptions);
                var extension = problemDetails.GetProblemDetailsExtension();
            }
        }
        catch (Exception exception)
        {
            Log.Fatal("Fatal error occurred: {message}", exception.Message);
            Environment.Exit(1);
        }
    }

    public async Task<List<ChatTemplateSettingModel>> GetChatTemplateSettingModelsAsync()
    {
        List<ChatTemplateSettingModel> chatTemplateSettingModels = [];

        try
        {
            var httpResult = await _chatTemplateHttpClient.GetAvailableChatTemplatesAsync();

            if (httpResult.IsSuccessStatusCode)
            {
                var response = JsonSerializer.Deserialize<GetChatTemplatesQueryResponse>(httpResult.Content, _jsonSerializerOptions);
                ICollection<ChatTemplateDto> chatTemplateDtos = response!.ChatTemplateDtos;

                foreach (var dto in chatTemplateDtos)
                {
                    chatTemplateSettingModels.Add(new ChatTemplateSettingModel(dto));
                }
            }
            else
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, _jsonSerializerOptions);
                var extension = problemDetails.GetProblemDetailsExtension();
                Log.Warning("Error {message}: ", extension.Errors.First().Message);
            }

            return chatTemplateSettingModels;
        }
        catch (Exception exception)
        {
            Log.Fatal("Fatal error occurred: {message}", exception.Message);

            return chatTemplateSettingModels;
        }
    }

    private void SetChatTemplates(ICollection<ChatTemplateDto> chatTemplateDtos)
    {
        _chatTemplateModels.Clear();

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
