#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales;

public class ValidationProcess
{
    public ValidationProcess()
    {
        ParameterValidasi = new ValidationParameter();
    }

    public DateTime TglChatCallMulai { get; set; }
    public DateTime TglChatCallRespons { get; set; }
    public string LinkRecapChatHistory { get; set; }
    public ValidationParameter ParameterValidasi { get; set; }
    public ValidationStatus StatusValidasi { get; set; }
    public string Keterangan { get; set; }
}
