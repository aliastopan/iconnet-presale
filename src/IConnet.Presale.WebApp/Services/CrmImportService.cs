using IConnet.Presale.Shared.Interfaces.Models.Presales;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Services;

public sealed class CrmImportService
{
    private readonly List<IApprovalOpportunityModel> _importModels;
    private readonly IDateTimeService _dateTimeService;

    public CrmImportService(IDateTimeService dateTimeService)
    {
        _importModels = new List<IApprovalOpportunityModel>();
        _dateTimeService = dateTimeService;
    }

    public IQueryable<IApprovalOpportunityModel> ApprovalOpportunities => _importModels.AsQueryable();

    public List<IApprovalOpportunityModel> Import(string input)
    {
        int chunkSize = 28;
        char[] delimiters = new char[] { '\t', '\n' };
        string[] contents = input.Split(delimiters);

        bool isValid = contents.Length % chunkSize == 0;
        if (!isValid)
        {
            Log.Warning("Invalid row count of {0}", contents.Length);
            return new List<IApprovalOpportunityModel>();
        }

        int chunkTotal = contents.Length / chunkSize;
        var importModels = new List<ImportModel>();

        for (int i = 0; i < chunkTotal; i++)
        {
            string[] chunk = contents.Skip(i * chunkSize).Take(chunkSize).ToArray();
            var importModel = CreateImportModel(chunk);

            if (!_importModels.Any(crm => crm.IdPermohonan == importModel.IdPermohonan))
            {
                importModels.Add(importModel);
            }
        }

        _importModels.AddRange(importModels);
        return _importModels;
    }

    private ImportModel CreateImportModel(string[] column)
    {
        return new ImportModel
        {
            IdPermohonan = column[0],
            TglPermohonan = column[1],
            DurasiTidakLanjut = column[2],
            NamaPemohon = column[3],
            IdPln = column[4],
            Layanan = column[5],
            SumberPermohonan = column[6],
            StatusPermohonan = column[7],
            NamaAgen = column[8],
            EmailAgen = column[9],
            TeleponAgen = column[10],
            MitraAgen = column[11],
            Splitter = column[12],
            JenisPermohonan = column[13],
            TeleponPemohon = column[14],
            EmailPemohon = column[15],
            NikPemohon = column[16],
            NpwpPemohon = column[17],
            Keterangan = column[18],
            AlamatPemohon = column[19],
            Regional = column[20],
            KantorPerwakilan = column[21],
            Provinsi = column[22],
            Kabupaten = column[23],
            Kecamatan = column[24],
            Kelurahan = column[25],
            Latitude = column[26],
            Longitude = column[27],
            TglImport = _dateTimeService.DateTimeOffsetNow,
            NamaClaimImport = "PAC"
        };
    }

    public string[] SplitBySpecialCharacters(string input)
    {
        char[] delimiters = new char[] { '\t', '\n' };
        return input.Split(delimiters);
    }

    public int EmptySplitCount(string[] strings)
    {
        int counter = 0;
        foreach (var col in strings)
        {
            if (string.IsNullOrWhiteSpace(col))
            {
                counter++;
            }
        }

        return counter;
    }

    public void CountSpecialCharacters(string input, out int tabCount, out int newlineCount)
    {
        tabCount = 0;
        newlineCount = 0;
        foreach (char c in input)
        {
            if (c == '\t')
            {
                tabCount++;
            }
            else if (c == '\n')
            {
                newlineCount++;
            }
        }
    }
}
