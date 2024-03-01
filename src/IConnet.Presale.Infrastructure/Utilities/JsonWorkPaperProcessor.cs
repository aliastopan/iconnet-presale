using System.Collections.Concurrent;
using System.Text.Json;
using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Domain.Aggregates.Presales;

namespace IConnet.Presale.Infrastructure.Utilities;

internal static class JsonWorkPaperProcessor
{
    internal static IEnumerable<string> FilterJsonWorkPapers(IEnumerable<string> jsonWorkPapers, WorkloadFilter filter,
        ParallelOptions parallelOptions, int parallelThreshold = 100)
    {
        var concurrentQueue = new ConcurrentQueue<string>();

        if (jsonWorkPapers.Count() > parallelThreshold)
        {
            Parallel.ForEach(jsonWorkPapers, parallelOptions, json =>
            {
                if (json != null && ShouldOnlyInclude(json, filter))
                {
                    concurrentQueue.Enqueue(json);
                }
            });
        }
        else
        {
            foreach (var json in jsonWorkPapers)
            {
                if (json != null && ShouldOnlyInclude(json, filter))
                {
                    concurrentQueue.Enqueue(json);
                }
            }
        }

        return concurrentQueue.ToList();
    }

    internal static IEnumerable<WorkPaper> DeserializeJsonWorkPapers(IEnumerable<string> jsonWorkPapers,
        ParallelOptions parallelOptions, int parallelThreshold = 100)
    {
        var concurrentBag = new ConcurrentBag<WorkPaper>();

        if (jsonWorkPapers.Count() > parallelThreshold)
        {
            Parallel.ForEach(jsonWorkPapers, parallelOptions, json =>
            {
                try
                {
                    if (json is null)
                    {
                        throw new JsonException();
                    }

                    var workPaper = JsonSerializer.Deserialize<WorkPaper>(json);
                    concurrentBag.Add(workPaper!);
                }
                catch (JsonException exception)
                {
                    Log.Fatal("Error deserializing JSON: {message}", exception.Message);
                }
            });
        }
        else
        {
            foreach (var json in jsonWorkPapers)
            {
                try
                {
                    if (json is null)
                    {
                        throw new JsonException();
                    }

                    var workPaper = JsonSerializer.Deserialize<WorkPaper>(json);
                    concurrentBag.Add(workPaper!);
                }
                catch (JsonException exception)
                {
                    Log.Fatal("Error deserializing JSON: {message}", exception.Message);
                }
            }
        }

        return concurrentBag.ToList();
    }

    internal static bool ShouldOnlyInclude(string json, WorkloadFilter filter)
    {
        var workPaperLevel = ExtractWorkPaperLevelFromJson(json);

        return filter switch
        {
            WorkloadFilter.OnlyImportUnverified => workPaperLevel == WorkPaperLevel.ImportUnverified,
            WorkloadFilter.OnlyImportInvalid => workPaperLevel == WorkPaperLevel.ImportInvalid,
            WorkloadFilter.OnlyImportArchived => workPaperLevel == WorkPaperLevel.ImportArchived,
            WorkloadFilter.OnlyImportVerified => workPaperLevel == WorkPaperLevel.ImportVerified,
            WorkloadFilter.OnlyValidating => (workPaperLevel & (WorkPaperLevel.Validating | WorkPaperLevel.ImportVerified)) != 0,
            WorkloadFilter.OnlyWaitingApproval => workPaperLevel == WorkPaperLevel.WaitingApproval,
            WorkloadFilter.OnlyDoneProcessing => workPaperLevel == WorkPaperLevel.DoneProcessing,
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
