namespace IConnet.Presale.WebApp.Components.Settings.SLA;

public partial class SlaSetting : ComponentBase
{
    [Inject] public AppSettingsService AppSettingsService { get; set; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;

    protected string TotalMinutesSlaImport => $"{(int)AppSettingsService.SlaImport.TotalMinutes}";
    protected string TotalMinutesSlaPickUp => $"{(int)AppSettingsService.SlaPickUp.TotalMinutes}";
    protected string TotalMinutesSlaValidasi => $"{(int)AppSettingsService.SlaValidasi.TotalMinutes}";
    protected string TotalMinutesSlaApproval => $"{(int)AppSettingsService.SlaApproval.TotalMinutes}";

    protected bool EnableSlaEdit { get; set;}
    protected bool DisableEdit => !EnableSlaEdit;

    protected int NewSlaImport { get; set; }
    protected int NewSlaPickUp { get; set; }
    protected int NewSlaValidasi { get; set; }
    protected int NewSlaApproval { get; set; }

    protected override void OnInitialized()
    {
        NewSlaImport = (int)AppSettingsService.SlaImport.TotalMinutes;
        NewSlaPickUp = (int)AppSettingsService.SlaPickUp.TotalMinutes;
        NewSlaValidasi = (int)AppSettingsService.SlaValidasi.TotalMinutes;
        NewSlaApproval = (int)AppSettingsService.SlaApproval.TotalMinutes;
    }

    protected void OnNewSlaImportChanged(int slaImport)
    {
        NewSlaImport = slaImport;

        LogSwitch.Debug("Change SLA Import");
    }

    protected void OnNewSlaPickUpChanged(int slaPickUp)
    {
        NewSlaPickUp = slaPickUp;

        LogSwitch.Debug("Change SLA Pick-Up");
    }

    protected void OnNewSlaValidasiChanged(int slaValidasi)
    {
        NewSlaValidasi = slaValidasi;

        LogSwitch.Debug("Change SLA Validasi");
    }

    protected void OnNewSlaApprovalChanged(int slaApproval)
    {
        NewSlaApproval = slaApproval;

        LogSwitch.Debug("Change SLA Approval");
    }

    protected async Task SaveSlaImportAsync()
    {
        TimeSpan newSlaImport = TimeSpan.FromMinutes(NewSlaImport);

        await AppSettingsService.EditSlaImportAsync(newSlaImport);

        EditSlaToast("Import");
    }

    protected async Task SaveSlaPickUpAsync()
    {
        TimeSpan newSlaPickUp = TimeSpan.FromMinutes(NewSlaPickUp);

        await AppSettingsService.EditSlaPickUpAsync(newSlaPickUp);

        EditSlaToast("Pick-Up");
    }

    protected async Task SaveSlaValidasiAsync()
    {
        TimeSpan newSlaValidasi = TimeSpan.FromMinutes(NewSlaValidasi);

        await AppSettingsService.EditSlaValidasiAsync(newSlaValidasi);

        EditSlaToast("Validasi");
    }

    protected async Task SaveSlaApprovalAsync()
    {
        TimeSpan newSlaApproval = TimeSpan.FromMinutes(NewSlaApproval);

        await AppSettingsService.EditSlaApprovalAsync(newSlaApproval);

        EditSlaToast("Approval");
    }

    private void EditSlaToast(string slaName)
    {
        var intent = ToastIntent.Success;
        var message = $"SLA {slaName} telah berhasil diubah.";
        var timeout = 5000;

        ToastService.ShowToast(intent, message, timeout);
    }
}
