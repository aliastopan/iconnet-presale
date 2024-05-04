using System.Text.Json;

namespace IConnet.Presale.WebApp.Services;

public class AppSettingsService
{
    private readonly IPresaleAppService _presaleAppService;

    private string _chatTemplate = default!;

    private TimeOnly _officeHourPagiStart;
    private TimeOnly _officeHourPagiEnd;
    private TimeOnly _officeHourMalamStart;
    private TimeOnly _officeHourMalamEnd;

    private TimeSpan _slaImport;
    private TimeSpan _slaPickUp;
    private TimeSpan _slaValidasi;
    private TimeSpan _slaApproval;

    private List<string> _rootCauseClassifications = [];

    public string ChatTemplate => _chatTemplate;

    public TimeOnly OfficeHourPagiStart => _officeHourPagiStart;
    public TimeOnly OfficeHourPagiEnd => _officeHourPagiEnd;
    public TimeOnly OfficeHourMalamStart => _officeHourMalamStart;
    public TimeOnly OfficeHourMalamEnd => _officeHourMalamEnd;

    public TimeOnly ShiftStart => _officeHourPagiStart;
    public TimeOnly ShiftEnd => _officeHourMalamEnd;

    public TimeSpan SlaImport => _slaImport;
    public TimeSpan SlaPickUp => _slaPickUp;
    public TimeSpan SlaValidasi => _slaValidasi;
    public TimeSpan SlaApproval => _slaApproval;

    public List<string> RootCauseClassifications => _rootCauseClassifications;

    public AppSettingsService(IPresaleAppService presaleAppService)
    {
        _presaleAppService = presaleAppService;
    }

    public async Task SetDefaultPresaleSettings()
    {
        await _presaleAppService.SetDefaultSettingAsync();
    }

    public async Task SynchronizeAppSettingsAsync()
    {
        Log.Information("Synchronize App Settings");

        var key = "PRESALE_APP";
        var jsonSettings = await _presaleAppService.GetSettingValueAsync(key);

        var settings = JsonSerializer.Deserialize<PresaleSettingModel>(jsonSettings)!;

        _chatTemplate = settings.ChatTemplate;

        _officeHourPagiStart = settings.OfficeHoursPagi.Start;
        _officeHourPagiEnd = settings.OfficeHoursPagi.End;
        _officeHourMalamStart = settings.OfficeHoursMalam.Start;
        _officeHourMalamEnd = settings.OfficeHoursMalam.End;

        _slaImport = settings.ServiceLevelAgreement.Import;
        _slaPickUp = settings.ServiceLevelAgreement.PickUp;
        _slaValidasi = settings.ServiceLevelAgreement.Validasi;
        _slaApproval = settings.ServiceLevelAgreement.Approval;

        _rootCauseClassifications = settings.RootCauseClassification;
    }
}
