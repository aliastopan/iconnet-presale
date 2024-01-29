#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ValidationProcess : ValueObject
{
    public ValidationProcess()
    {
        ChatCallMulai = new ActionSignature();
        ChatCallRespons = new ActionSignature();
        ParameterValidasi = new ValidationParameter();
    }

    public ValidationProcess(ActionSignature chatCallMulai, ActionSignature chatCallRespons, string linkRecapChatHistory,
        ValidationParameter parameterValidasi, ValidationCorrection koreksiValidasi, ValidationStatus statusValidasi, string keterangan)
    {
        ChatCallMulai = chatCallMulai;
        ChatCallRespons = chatCallRespons;
        LinkRecapChatHistory = linkRecapChatHistory;
        ParameterValidasi = parameterValidasi;
        KoreksiValidasi = koreksiValidasi;
        StatusValidasi = statusValidasi;
        Keterangan = keterangan;
    }

    public ActionSignature ChatCallMulai { get; init;}
    public ActionSignature ChatCallRespons { get; init;}
    public string LinkRecapChatHistory { get; init; }
    public ValidationParameter ParameterValidasi { get; init; }
    public ValidationCorrection KoreksiValidasi { get; set; }
    public ValidationStatus StatusValidasi { get; init; }
    public string Keterangan { get; init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ChatCallMulai;
        yield return ChatCallRespons;
        yield return LinkRecapChatHistory;
        yield return ParameterValidasi;
        yield return KoreksiValidasi;
        yield return StatusValidasi;
        yield return Keterangan;
    }
}
