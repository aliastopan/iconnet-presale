using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Services;

public class StartupService : IHostedService
{
    private readonly IWorkloadSynchronizationManager _workloadSynchronizationManager;
    private readonly UserManager _userManager;
    private readonly ChatTemplateManager _chatTemplateManager;
    private readonly RepresentativeOfficeManager _representativeOfficeManager;
    private readonly RootCauseManager _rootCauseManager;

    public StartupService(IWorkloadSynchronizationManager workloadSynchronizationManager,
        UserManager userManager,
        ChatTemplateManager chatTemplateManager,
        RepresentativeOfficeManager representativeOfficeManager,
        RootCauseManager rootCauseManager)
    {
        _workloadSynchronizationManager = workloadSynchronizationManager;
        _userManager = userManager;
        _chatTemplateManager = chatTemplateManager;
        _representativeOfficeManager = representativeOfficeManager;
        _rootCauseManager = rootCauseManager;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Task[] tasks =
        [
            _chatTemplateManager.SetDefaultChatTemplatesAsync(),
            _representativeOfficeManager.SetRepresentativeOfficesAsync(),
            _rootCauseManager.SetRootCausesAsync(),
            _userManager.SetPresaleOperatorsAsync(),
            PullRedisToInMemoryAsync()
        ];

        await Task.WhenAll(tasks);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Log.Information("Closing application");

        return Task.CompletedTask;
    }

    private async Task PullRedisToInMemoryAsync()
    {
        await _workloadSynchronizationManager.PullRedisToInMemoryAsync();
    }
}
