namespace IConnet.Presale.WebApp.Services;

public class AppSettingsService
{
    private TimeOnly _officeHourPagiStart;
    private TimeOnly _officeHourPagiEnd;
    private TimeOnly _officeHourMalamStart;
    private TimeOnly _officeHourMalamEnd;

    private TimeSpan _slaImport;
    private TimeSpan _slaPickUp;
    private TimeSpan _slaValidasi;
    private TimeSpan _slaApproval;

    public AppSettingsService(IConfiguration configuration)
    {
        string officeHourPagiStartString = configuration["OfficeHours:Pagi:Start"]!;
        string officeHourPagiEndString = configuration["OfficeHours:Pagi:End"]!;
        string officeHourMalamStartString = configuration["OfficeHours:Malam:Start"]!;
        string officeHourMalamEndString = configuration["OfficeHours:Malam:End"]!;

        _officeHourPagiStart = TimeOnly.ParseExact(officeHourPagiStartString, "HH:mm:ss", null);
        _officeHourPagiEnd = TimeOnly.ParseExact(officeHourPagiEndString, "HH:mm:ss", null);
        _officeHourMalamStart = TimeOnly.ParseExact(officeHourMalamStartString, "HH:mm:ss", null);
        _officeHourMalamEnd = TimeOnly.ParseExact(officeHourMalamEndString, "HH:mm:ss", null);

        string slaImportString = configuration["ServiceLevelAgreement:Import"]!;
        string slaPickUpString = configuration["ServiceLevelAgreement:PickUp"]!;
        string slaValidasiString = configuration["ServiceLevelAgreement:Validasi"]!;
        string slaApprovalString = configuration["ServiceLevelAgreement:Approval"]!;

        _slaImport = TimeSpan.Parse(slaImportString);
        _slaPickUp = TimeSpan.Parse(slaPickUpString);
        _slaValidasi = TimeSpan.Parse(slaValidasiString);
        _slaApproval = TimeSpan.Parse(slaApprovalString);
    }

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
}