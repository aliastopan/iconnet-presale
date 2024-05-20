#nullable disable

namespace IConnet.Presale.Application.Common.Models;

public class PresaleSettingModel
{
    public string ChatTemplate { get; set; }
    public OfficeHours OfficeHoursPagi { get; set; }
    public OfficeHours OfficeHoursMalam { get; set; }
    public ServiceLevelAgreement ServiceLevelAgreement { get; set; }
    public List<string> RootCauseClassification { get; set; }
}

public class OfficeHours
{
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }
}

public class ServiceLevelAgreement
{
    public TimeSpan Import { get; set; }
    public TimeSpan PickUp { get; set; }
    public TimeSpan Validasi { get; set; }
    public TimeSpan Approval { get; set; }
}