namespace IConnet.Presale.WebApp.Services;

public class StartupService : IHostedService
{
    private readonly AppSettingsService _appSettingsService;
    private readonly IPresaleAppService _presaleAppService;
    private readonly IWorkloadSynchronizationManager _workloadSynchronizationManager;
    private readonly UserManager _userManager;
    private readonly ChatTemplateManager _chatTemplateManager;
    private readonly DirectApprovalManager _directApprovalManager;
    private readonly RepresentativeOfficeManager _representativeOfficeManager;
    private readonly RootCauseManager _rootCauseManager;
    private readonly CommonDuplicateService _commonDuplicateService;
    private readonly IPresaleDataBoundaryManager _presaleDataBoundaryManager;

    public StartupService(AppSettingsService appSettingsService,
        IPresaleAppService presaleAppService,
        IWorkloadSynchronizationManager workloadSynchronizationManager,
        UserManager userManager,
        ChatTemplateManager chatTemplateManager,
        DirectApprovalManager directApprovalManager,
        RepresentativeOfficeManager representativeOfficeManager,
        RootCauseManager rootCauseManager,
        CommonDuplicateService commonDuplicateService,
        IPresaleDataBoundaryManager presaleDataBoundaryManager)
    {
        _appSettingsService = appSettingsService;
        _presaleAppService = presaleAppService;
        _workloadSynchronizationManager = workloadSynchronizationManager;
        _userManager = userManager;
        _chatTemplateManager = chatTemplateManager;
        _directApprovalManager = directApprovalManager;
        _representativeOfficeManager = representativeOfficeManager;
        _rootCauseManager = rootCauseManager;
        _commonDuplicateService = commonDuplicateService;
        _presaleDataBoundaryManager = presaleDataBoundaryManager;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var IsNullSettings = await _presaleAppService.IsNullSettings();

        if (IsNullSettings)
        {
            await _presaleAppService.SetDefaultSettingAsync();
        }

        await _appSettingsService.InitAppSettingsAsync();

        Task[] tasks =
        [
            _chatTemplateManager.SetDefaultChatTemplatesAsync(),
            _directApprovalManager.SetDirectApprovalsAsync(),
            _representativeOfficeManager.SetRepresentativeOfficesAsync(),
            _rootCauseManager.SetRootCausesAsync(),
            _userManager.SetPresaleOperatorsAsync(),
            PullRedisToInMemoryAsync()
        ];

        await InitCollectPotentialDuplicate();

        await Task.WhenAll(tasks);
    }

    private async Task InitCollectPotentialDuplicate()
    {
        IQueryable<WorkPaper>? presaleData = await _presaleDataBoundaryManager.GetBoundaryChunkPresaleDataAsync(offset: 31);
        _commonDuplicateService.SetCommonDuplicates(presaleData);

        Log.Information("Init potential duplicate list");
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
