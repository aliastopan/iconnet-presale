#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ValidationProcess : ValueObject
{
    public ValidationProcess()
    {

    }

    public ValidationProcess(ActionSignature chatCallMulai, ActionSignature chatCallRespons, string linkRecapChatHistory,
        ValidationParameter parameterValidasi, ValidationCorrection koreksiValidasi, ValidationStatus statusValidasi, string keterangan)
    {
        ChatCallMulai = chatCallMulai;
        ChatCallRespons = chatCallRespons;
        LinkRecapChatHistory = linkRecapChatHistory;
        ParameterValidasi = parameterValidasi;
        PembetulanValidasi = koreksiValidasi;
        StatusValidasi = statusValidasi;
        Keterangan = keterangan;
    }

    public ActionSignature ChatCallMulai { get; init;} = new();
    public ActionSignature ChatCallRespons { get; init;} = new();
    public string LinkRecapChatHistory { get; init; } = string.Empty;
    public ValidationParameter ParameterValidasi { get; init; } = new();
    public ValidationCorrection PembetulanValidasi { get; set; } = new();
    public ValidationStatus StatusValidasi { get; init; } = default;
    public string Keterangan { get; init; }  = string.Empty;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ChatCallMulai;
        yield return ChatCallRespons;
        yield return LinkRecapChatHistory;
        yield return ParameterValidasi;
        yield return PembetulanValidasi;
        yield return StatusValidasi;
        yield return Keterangan;
    }
}
