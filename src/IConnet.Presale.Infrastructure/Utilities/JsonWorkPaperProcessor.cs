using System.Text.Json;
using IConnet.Presale.Domain.Enums;

namespace IConnet.Presale.Infrastructure.Utilities;

internal static class JsonWorkPaperProcessor
{
    internal static bool ShouldOnlyInclude(string json, CacheFetchMode cacheFetchMode)
    {
        var workPaperLevel = ExtractWorkPaperLevelFromJson(json);

        return cacheFetchMode switch
        {
            CacheFetchMode.OnlyImportUnverified => workPaperLevel == WorkPaperLevel.ImportUnverified,
            CacheFetchMode.OnlyImportInvalid => workPaperLevel == WorkPaperLevel.ImportInvalid,
            CacheFetchMode.OnlyImportArchived => workPaperLevel == WorkPaperLevel.ImportArchived,
            CacheFetchMode.OnlyImportVerified => workPaperLevel == WorkPaperLevel.ImportVerified,
            CacheFetchMode.OnlyValidating => (workPaperLevel & (WorkPaperLevel.Validating | WorkPaperLevel.ImportVerified)) != 0,
            CacheFetchMode.OnlyWaitingApproval => workPaperLevel == WorkPaperLevel.WaitingApproval,
            CacheFetchMode.OnlyDoneProcessing => workPaperLevel == WorkPaperLevel.DoneProcessing,
            _ => true,
        };
    }

    internal static WorkPaperLevel ExtractWorkPaperLevelFromJson(string json)
    {
        try
        {
            var jsonDocument = JsonDocument.Parse(json);
            var root = jsonDocument.RootElement;

            if (root.TryGetProperty("WorkPaperLevel", out JsonElement workPaperLevelElement))
            {
                if (workPaperLevelElement.TryGetInt32(out int workPaperLevelInt))
                {
                    return (WorkPaperLevel)workPaperLevelInt;
                }
            }

            throw new InvalidOperationException("WorkPaperLevel could not be extracted from the JSON string.");
        }
        catch (JsonException exception)
        {
            Log.Fatal("Error parsing JSON: {message}", exception.Message);
            throw;
        }
        catch (Exception exception)
        {
            Log.Fatal("An error occurred: {message}", exception.Message);
            throw;
        }
    }
}
