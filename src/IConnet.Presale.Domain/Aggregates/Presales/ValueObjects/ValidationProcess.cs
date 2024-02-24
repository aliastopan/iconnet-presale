#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ValidationProcess : ValueObject
{
    public ValidationProcess()
    {

    }

    public ValidationProcess(ActionSignature chatCallMulai, ActionSignature chatCallRespons, DateTime waktuTanggalRespons,
        string linkRekapChatHistory, ValidationParameter parameterValidasi, ValidationCorrection pembetulanValidasi, string keterangan)
    {
        ChatCallMulai = chatCallMulai;
        ChatCallRespons = chatCallRespons;
        WaktuTanggalRespons = waktuTanggalRespons;
        LinkRekapChatHistory = linkRekapChatHistory;
        ParameterValidasi = parameterValidasi;
        PembetulanValidasi = pembetulanValidasi;
        Keterangan = keterangan;
    }

    public ActionSignature ChatCallMulai { get; init;} = new();
    public ActionSignature ChatCallRespons { get; init;} = new();
    public DateTime WaktuTanggalRespons { get; init; } = DateTime.MinValue;
    public string LinkRekapChatHistory { get; init; } = string.Empty;
    public ValidationParameter ParameterValidasi { get; init; } = new();
    public ValidationCorrection PembetulanValidasi { get; init; } = new();
    public string Keterangan { get; init; }  = string.Empty;

    [NotMapped]
    public bool IsOnGoing => ChatCallRespons.IsEmptySignature();

    public ValidationProcess WithChatCallMulai(ActionSignature chatCallMulai)
    {
       return new ValidationProcess
        {
            ChatCallMulai = chatCallMulai,
            ChatCallRespons = this.ChatCallRespons,
            WaktuTanggalRespons = this.WaktuTanggalRespons,
            LinkRekapChatHistory = this.LinkRekapChatHistory,
            ParameterValidasi = this.ParameterValidasi,
            PembetulanValidasi = this.PembetulanValidasi,
            Keterangan = this.Keterangan
        };
    }

    public ValidationProcess WithChatCallRespons(ActionSignature chatCallRespons)
    {
       return new ValidationProcess
        {
            ChatCallMulai = this.ChatCallMulai,
            ChatCallRespons = chatCallRespons,
            WaktuTanggalRespons = this.WaktuTanggalRespons,
            LinkRekapChatHistory = this.LinkRekapChatHistory,
            ParameterValidasi = this.ParameterValidasi,
            PembetulanValidasi = this.PembetulanValidasi,
            Keterangan = this.Keterangan
        };
    }

    public ValidationProcess WithWaktuTanggalRespons(DateTime waktuTanggalRespons)
    {
       return new ValidationProcess
        {
            ChatCallMulai = this.ChatCallMulai,
            ChatCallRespons = this.ChatCallRespons,
            WaktuTanggalRespons = waktuTanggalRespons,
            LinkRekapChatHistory = this.LinkRekapChatHistory,
            ParameterValidasi = this.ParameterValidasi,
            PembetulanValidasi = this.PembetulanValidasi,
            Keterangan = this.Keterangan
        };
    }

    public ValidationProcess WithLinkRekapChatHistory(string linkRekapChatHistory)
    {
       return new ValidationProcess
        {
            ChatCallMulai = this.ChatCallMulai,
            ChatCallRespons = this.ChatCallRespons,
            WaktuTanggalRespons = this.WaktuTanggalRespons,
            LinkRekapChatHistory = linkRekapChatHistory,
            ParameterValidasi = this.ParameterValidasi,
            PembetulanValidasi = this.PembetulanValidasi,
            Keterangan = this.Keterangan
        };
    }

    public ValidationProcess WithParameterValidasi(ValidationParameter parameterValidasi)
    {
        return new ValidationProcess
        {
            ChatCallMulai = this.ChatCallMulai,
            ChatCallRespons = this.ChatCallRespons,
            WaktuTanggalRespons = this.WaktuTanggalRespons,
            LinkRekapChatHistory = this.LinkRekapChatHistory,
            ParameterValidasi = parameterValidasi,
            PembetulanValidasi = this.PembetulanValidasi,
            Keterangan = this.Keterangan
        };
    }

    public ValidationProcess WithPembetulanValidasi(ValidationCorrection pembetulanValidasi)
    {
        return new ValidationProcess
        {
            ChatCallMulai = this.ChatCallMulai,
            ChatCallRespons = this.ChatCallRespons,
            WaktuTanggalRespons = this.WaktuTanggalRespons,
            LinkRekapChatHistory = this.LinkRekapChatHistory,
            ParameterValidasi = this.ParameterValidasi,
            PembetulanValidasi = pembetulanValidasi,
            Keterangan = this.Keterangan
        };
    }

    public ValidationProcess WithKeterangan(string keterangan)
    {
        return new ValidationProcess
        {
            ChatCallMulai = this.ChatCallMulai,
            ChatCallRespons = this.ChatCallRespons,
            WaktuTanggalRespons = this.WaktuTanggalRespons,
            LinkRekapChatHistory = this.LinkRekapChatHistory,
            ParameterValidasi = this.ParameterValidasi,
            PembetulanValidasi = this.PembetulanValidasi,
            Keterangan = keterangan
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ChatCallMulai;
        yield return ChatCallRespons;
        yield return WaktuTanggalRespons;
        yield return LinkRekapChatHistory;
        yield return ParameterValidasi;
        yield return PembetulanValidasi;
        yield return Keterangan;
    }
}
