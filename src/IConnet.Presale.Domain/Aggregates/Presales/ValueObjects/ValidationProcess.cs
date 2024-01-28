#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ValidationProcess
{
    public ValidationProcess()
    {
        ChatCallMulai = new ActionSignature();
        ChatCallRespons = new ActionSignature();
        ParameterValidasi = new ValidationParameter();
    }

    public ActionSignature ChatCallMulai { get; set;}
    public ActionSignature ChatCallRespons { get; set;}
    public string LinkRecapChatHistory { get; set; }
    public ValidationParameter ParameterValidasi { get; set; }
    public ValidationStatus StatusValidasi { get; set; }
    public string Keterangan { get; set; }
}
