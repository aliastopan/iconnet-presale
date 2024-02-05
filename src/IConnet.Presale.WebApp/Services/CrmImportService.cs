using IConnet.Presale.Shared.Interfaces.Models.Presales;
using IConnet.Presale.WebApp.Helpers;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Services;

public sealed class CrmImportService
{
    private const int NumberOfColumn = 28;
    private readonly List<IApprovalOpportunityModel> _importModels;
    private readonly IDateTimeService _dateTimeService;
    private readonly SessionService _sessionService;

    public CrmImportService(IDateTimeService dateTimeService,
        SessionService sessionService)
    {
        _importModels = new List<IApprovalOpportunityModel>();
        _dateTimeService = dateTimeService;
        _sessionService = sessionService;
    }

    public IQueryable<IApprovalOpportunityModel> GetApprovalOpportunities()
    {
        return _importModels.OrderByDescending(x => x.TglImport).AsQueryable();
    }

    public async Task<(List<IApprovalOpportunityModel>, CrmImportMetadata)> ImportAsync(string input)
    {
        string[] contents = SplitBySpecialCharacters(input);
        var importMetadata = GetImportMetadata(input, contents);

        bool isValid = contents.Length % NumberOfColumn == 0;
        if (!isValid)
        {
            // Log.Warning("Invalid row count of {0}", contents.Length);
            return (new List<IApprovalOpportunityModel>(), new CrmImportMetadata());
        }

        int numberOfRows = contents.Length / NumberOfColumn;
        var importModels = new List<ImportModel>();

        for (int i = 0; i < numberOfRows; i++)
        {
            string[] rowData = contents.Skip(i * NumberOfColumn).Take(NumberOfColumn).ToArray();
            var importModel = await CreateImportModelAsync(rowData);

            var hasDuplicate = _importModels.Any(crm => crm.IdPermohonan == importModel.IdPermohonan);
            if (hasDuplicate)
            {
                importMetadata.NumberOfDuplicates++;
                continue;
            }

            importModels.Add(importModel);
        }

        _importModels.AddRange(importModels);

        return (_importModels, importMetadata);
    }

    private async Task<ImportModel> CreateImportModelAsync(string[] column)
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
            TglImport = _dateTimeService.DateTimeOffsetNow.DateTime,
            ImportAccountIdSignature = await _sessionService.GetUserAccountIdAsync(),
            ImportAliasSignature = await _sessionService.GetSessionAliasAsync()
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
