using System.Linq;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Services;

public sealed class CrmImportService
{
    private readonly List<ImportModel> _importModels;

    public CrmImportService()
    {
        _importModels = new List<ImportModel>();
    }

    public IQueryable<ImportModel> ApprovalOpportunities => _importModels.AsQueryable();

    public void Import(string input)
    {
        char[] delimiters = new char[] { '\t', '\n' };
        string[] contents = input.Split(delimiters);

        bool isValid = contents.Length % 28 == 0;
        if (!isValid)
        {
            Log.Warning("Invalid row count of {0}", contents.Length);
            return;
        }

        var importModel = new ImportModel
        {
            IdPermohonan = contents[0],
            TglPermohonan = contents[1],
            DurasiTidakLanjut = contents[2],
            NamaPemohon = contents[3],
            IdPln = contents[4],
            Layanan = contents[5],
            SumberPermohonan = contents[6],
            StatusPermohonan = contents[7],
            NamaAgen = contents[8],
            EmailAgen = contents[9],
            TeleponAgen = contents[10],
            MitraAgen = contents[11],
            Splitter = contents[12],
            JenisPermohonan = contents[13],
            TeleponPemohon = contents[14],
            EmailPemohon = contents[15],
            NikPemohon = contents[16],
            NpwpPemohon = contents[17],
            Keterangan = contents[18],
            AlamatPemohon = contents[19],
            Regional = contents[20],
            KantorPerwakilan = contents[21],
            Provinsi = contents[22],
            Kabupaten = contents[23],
            Kecamatan = contents[24],
            Kelurahan = contents[25],
            Latitude = contents[26],
            Longitude = contents[27]
        };

        _importModels.Add(importModel);
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
