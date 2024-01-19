using IConnet.Presale.Shared.Interfaces.Models.Presales;
using IConnet.Presale.WebApp.Helpers;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Services;

public sealed class CrmImportService
{
    private const int NumberOfColumn = 28;
    private readonly List<IApprovalOpportunityModel> _importModels;
    private readonly IDateTimeService _dateTimeService;

    public CrmImportService(IDateTimeService dateTimeService)
    {
        _importModels = new List<IApprovalOpportunityModel>();
        _dateTimeService = dateTimeService;
    }

    public IQueryable<IApprovalOpportunityModel> ApprovalOpportunities => _importModels.AsQueryable();

    public List<IApprovalOpportunityModel> Import(string input, out CrmImportMetadata importMetadata)
    {
        string[] contents = SplitBySpecialCharacters(input);
        importMetadata = GetImportMetadata(input, contents);

        bool isValid = contents.Length % NumberOfColumn == 0;
        if (!isValid)
        {
            Log.Warning("Invalid row count of {0}", contents.Length);
            return new List<IApprovalOpportunityModel>();
        }

        int numberOfRows = contents.Length / NumberOfColumn;
        var importModels = new List<ImportModel>();

        for (int i = 0; i < numberOfRows; i++)
        {
            string[] chunk = contents.Skip(i * NumberOfColumn).Take(NumberOfColumn).ToArray();
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

    private static CrmImportMetadata GetImportMetadata(string input, string[] contents)
    {
        return new CrmImportMetadata
        {
            StringLength = input.Length,
            NumberOfWhiteSpaces = contents.Count(string.IsNullOrWhiteSpace),
            NumberOfTabSeparators = input.Count(c => c == '\t'),
            NumberOfRows = input.Count(c => c == '\n') + 1,
            IsValidImport = contents.Length % NumberOfColumn == 0
        };
    }

    private static string[] SplitBySpecialCharacters(string input)
    {
        char[] delimiters = ['\t', '\n'];
        return input.Split(delimiters);
    }
}
