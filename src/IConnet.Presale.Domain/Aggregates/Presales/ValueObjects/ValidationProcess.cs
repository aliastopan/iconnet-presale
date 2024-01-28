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
        ValidationParameter parameterValidasi, ValidationStatus statusValidasi, string keterangan)
    {
        ChatCallMulai = chatCallMulai;
        ChatCallRespons = chatCallRespons;
        LinkRecapChatHistory = linkRecapChatHistory;
        ParameterValidasi = parameterValidasi;
        StatusValidasi = statusValidasi;
        Keterangan = keterangan;
    }

    public ActionSignature ChatCallMulai { get; init;}
    public ActionSignature ChatCallRespons { get; init;}
    public string LinkRecapChatHistory { get; init; }
    public ValidationParameter ParameterValidasi { get; init; }
    public ValidationStatus StatusValidasi { get; init; }
    public string Keterangan { get; init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ChatCallMulai;
        yield return ChatCallRespons;
        yield return LinkRecapChatHistory;
        yield return ParameterValidasi;
        yield return StatusValidasi;
        yield return Keterangan;
    }
}
