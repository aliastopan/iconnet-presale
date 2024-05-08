using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Services;

public sealed class CrmImportService
{
    private const int NumberOfColumn = 27;
    private List<IApprovalOpportunityModel> _importModels;
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
        _importModels.Clear();

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

        var hashSetIds = new HashSet<string>();

        _importModels.AddRange(importModels);
        _importModels = _importModels.Where(model => hashSetIds.Add(model.IdPermohonan)).ToList();

        return (_importModels, importMetadata);
    }

    public async Task<(List<IApprovalOpportunityModel>, CrmImportMetadata)> ImportFromCsvAsync(List<string[]> csvInputs)
    {
        var importModels = new List<ImportModel>();
        var importMetadata = new CrmImportMetadata
        {
            NumberOfRows = csvInputs.Count,
            IsValidImport = true
        };

        foreach (var rowData in csvInputs)
        {
            var importModel = await CreateImportModelAsync(rowData);

            var hasDuplicate = _importModels.Any(crm => crm.IdPermohonan == importModel.IdPermohonan);
            if (hasDuplicate)
            {
                importMetadata.NumberOfDuplicates++;
                continue;
            }

            importModels.Add(importModel);
        }

        var hashSetIds = new HashSet<string>();

        _importModels.AddRange(importModels);
        _importModels = _importModels.Where(model => hashSetIds.Add(model.IdPermohonan)).ToList();

        return (_importModels, importMetadata);
    }

    private async Task<ImportModel> CreateImportModelAsync(string[] column)
    {
        return new ImportModel
        {
            IdPermohonan = column[0].SanitizeString(),
            TglPermohonan = column[1].SanitizeString(),
            DurasiTidakLanjut = column[2].SanitizeString(),
            NamaPemohon = column[3].SanitizeString(),
            IdPln = column[4].SanitizeString(),
            Layanan = column[5].SanitizeString(),
            SumberPermohonan = column[6].SanitizeString(),
            StatusPermohonan = column[7].SanitizeString(),
            NamaAgen = column[8].SanitizeString(),
            EmailAgen = column[9].SanitizeString(),
            TeleponAgen = column[10].SanitizeString(),
            MitraAgen = column[11].SanitizeString(),
            Splitter = column[12].SanitizeString(),
            JenisPermohonan = string.Empty,                         // dropped column
            TeleponPemohon = column[13].SanitizeString(),
            EmailPemohon = column[14].SanitizeString(),
            NikPemohon = column[15].SanitizeString(),
            NpwpPemohon = column[16].SanitizeString(),
            Keterangan = column[17].SanitizeString(),
            AlamatPemohon = column[18].SanitizeString(),
            Regional = column[19].SanitizeString(),
            KantorPerwakilan = column[20].SanitizeString(),
            Provinsi = column[21].SanitizeString(),
            Kabupaten = column[22].SanitizeString(),
            Kecamatan = column[23].SanitizeString(),
            Kelurahan = column[24].SanitizeString(),
            Latitude = column[25].SanitizeString(),
            Longitude = column[26].SanitizeString().RemoveNewlines(),
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
