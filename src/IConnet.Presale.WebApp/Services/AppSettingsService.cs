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

    public async Task SaveChangesAsync()
    {
        var setting = new PresaleSettingModel
        {
            ChatTemplate = this._chatTemplate,
            OfficeHoursPagi = new OfficeHours
            {
                Start = this._officeHourPagiStart,
                End = this._officeHourPagiEnd
            },
            OfficeHoursMalam = new OfficeHours
            {
                Start = this._officeHourMalamStart,
                End = this._officeHourMalamEnd
            },
            ServiceLevelAgreement = new ServiceLevelAgreement
            {
                Import = this._slaImport,
                PickUp = this._slaPickUp,
                Validasi = this._slaValidasi,
                Approval = this._slaApproval,
            },
            RootCauseClassification = this._rootCauseClassifications
        };

        var json = JsonSerializer.Serialize<PresaleSettingModel>(setting);
        var key = "PRESALE_APP";

        await _presaleAppService.SetSettingValueAsync(key, json);

        await SynchronizeAppSettingsAsync();
    }

    public void AddRootCauseClassification(string classification)
    {
        _rootCauseClassifications.Add(classification);
    }

    public async Task EditOfficeHourPagiAsync(TimeOnly officeHourPagiStart, TimeOnly officeHourPagiEnd)
    {
        _officeHourPagiStart = officeHourPagiStart;
        _officeHourPagiEnd = officeHourPagiEnd;

        await SaveChangesAsync();

        Log.Warning("Changing Pagi Office Hours");
    }

    public async Task EditOfficeHourMalamAsync(TimeOnly officeHourMalamStart, TimeOnly officeHourMalamEnd)
    {
        _officeHourMalamStart = officeHourMalamStart;
        _officeHourMalamEnd = officeHourMalamEnd;

        await SaveChangesAsync();
    }
}
