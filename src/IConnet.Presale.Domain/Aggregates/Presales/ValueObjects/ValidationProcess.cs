#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ValidationProcess : ValueObject
{
    public ValidationProcess()
    {

    }

    public ValidationProcess(ActionSignature signatureChatCallMulai, ActionSignature signatureChatCallRespons, DateTime waktuTanggalRespons,
        string linkChatHistory, ValidationParameter parameterValidasi, ValidationCorrection pembetulanValidasi, string keterangan)
    {
        SignatureChatCallMulai = signatureChatCallMulai;
        SignatureChatCallRespons = signatureChatCallRespons;
        WaktuTanggalRespons = waktuTanggalRespons;
        LinkChatHistory = linkChatHistory;
        ParameterValidasi = parameterValidasi;
        PembetulanValidasi = pembetulanValidasi;
        Keterangan = keterangan;
    }

    public ActionSignature SignatureChatCallMulai { get; init;} = new();
    public ActionSignature SignatureChatCallRespons { get; init;} = new();
    public DateTime WaktuTanggalRespons { get; init; } = DateTime.MinValue;
    public string LinkChatHistory { get; init; } = string.Empty;
    public ValidationParameter ParameterValidasi { get; init; } = new();
    public ValidationCorrection PembetulanValidasi { get; init; } = new();
    public string Keterangan { get; init; }  = string.Empty;

    public bool HasStarted()
    {
        return !SignatureChatCallMulai.IsEmptySignature();
    }

    public bool IsOnGoing()
    {
        return SignatureChatCallRespons.IsEmptySignature();
    }

    public ValidationProcess WithSignatureChatCallMulai(ActionSignature signatureChatCallMulai)
    {
       return new ValidationProcess
        {
            SignatureChatCallMulai = signatureChatCallMulai,
            SignatureChatCallRespons = this.SignatureChatCallRespons,
            WaktuTanggalRespons = this.WaktuTanggalRespons,
            LinkChatHistory = this.LinkChatHistory,
            ParameterValidasi = this.ParameterValidasi,
            PembetulanValidasi = this.PembetulanValidasi,
            Keterangan = this.Keterangan
        };
    }

    public ValidationProcess WithSignatureChatCallRespons(ActionSignature signatureChatCallRespons)
    {
       return new ValidationProcess
        {
            SignatureChatCallMulai = this.SignatureChatCallMulai,
            SignatureChatCallRespons = signatureChatCallRespons,
            WaktuTanggalRespons = this.WaktuTanggalRespons,
            LinkChatHistory = this.LinkChatHistory,
            ParameterValidasi = this.ParameterValidasi,
            PembetulanValidasi = this.PembetulanValidasi,
            Keterangan = this.Keterangan
        };
    }

    public ValidationProcess WithWaktuTanggalRespons(DateTime waktuTanggalRespons)
    {
       return new ValidationProcess
        {
            SignatureChatCallMulai = this.SignatureChatCallMulai,
            SignatureChatCallRespons = this.SignatureChatCallRespons,
            WaktuTanggalRespons = waktuTanggalRespons,
            LinkChatHistory = this.LinkChatHistory,
            ParameterValidasi = this.ParameterValidasi,
            PembetulanValidasi = this.PembetulanValidasi,
            Keterangan = this.Keterangan
        };
    }

    public ValidationProcess WithLinkChatHistory(string linkChatHistory)
    {
       return new ValidationProcess
        {
            SignatureChatCallMulai = this.SignatureChatCallMulai,
            SignatureChatCallRespons = this.SignatureChatCallRespons,
            WaktuTanggalRespons = this.WaktuTanggalRespons,
            LinkChatHistory = linkChatHistory,
            ParameterValidasi = this.ParameterValidasi,
            PembetulanValidasi = this.PembetulanValidasi,
            Keterangan = this.Keterangan
        };
    }

    public ValidationProcess WithParameterValidasi(ValidationParameter parameterValidasi)
    {
        return new ValidationProcess
        {
            SignatureChatCallMulai = this.SignatureChatCallMulai,
            SignatureChatCallRespons = this.SignatureChatCallRespons,
            WaktuTanggalRespons = this.WaktuTanggalRespons,
            LinkChatHistory = this.LinkChatHistory,
            ParameterValidasi = parameterValidasi,
            PembetulanValidasi = this.PembetulanValidasi,
            Keterangan = this.Keterangan
        };
    }

    public ValidationProcess WithPembetulanValidasi(ValidationCorrection pembetulanValidasi)
    {
        return new ValidationProcess
        {
            SignatureChatCallMulai = this.SignatureChatCallMulai,
            SignatureChatCallRespons = this.SignatureChatCallRespons,
            WaktuTanggalRespons = this.WaktuTanggalRespons,
            LinkChatHistory = this.LinkChatHistory,
            ParameterValidasi = this.ParameterValidasi,
            PembetulanValidasi = pembetulanValidasi,
            Keterangan = this.Keterangan
        };
    }

    public ValidationProcess WithKeterangan(string keterangan)
    {
        return new ValidationProcess
        {
            SignatureChatCallMulai = this.SignatureChatCallMulai,
            SignatureChatCallRespons = this.SignatureChatCallRespons,
            WaktuTanggalRespons = this.WaktuTanggalRespons,
            LinkChatHistory = this.LinkChatHistory,
            ParameterValidasi = this.ParameterValidasi,
            PembetulanValidasi = this.PembetulanValidasi,
            Keterangan = keterangan
        };
    }

    public bool IsClosedLost(DateTime today, int closedLostThreshold = 3)
    {
        TimeSpan agingResponse = GetAgingChatCallRespons(today);

        bool notResponding = SignatureChatCallRespons.IsEmptySignature();
        bool closedLost = Math.Abs(agingResponse.Days) >= closedLostThreshold;

        return notResponding && closedLost;
    }

    public TimeSpan GetAgingChatCallRespons(DateTime today)
    {
        return today - SignatureChatCallMulai.TglAksi;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SignatureChatCallMulai;
        yield return SignatureChatCallRespons;
        yield return WaktuTanggalRespons;
        yield return LinkChatHistory;
        yield return ParameterValidasi;
        yield return PembetulanValidasi;
        yield return Keterangan;
    }
}
