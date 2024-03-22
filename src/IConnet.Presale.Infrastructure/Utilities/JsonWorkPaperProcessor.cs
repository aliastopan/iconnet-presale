using System.Collections.Concurrent;
using System.Text.Json;
using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Domain.Aggregates.Presales;

namespace IConnet.Presale.Infrastructure.Utilities;

internal static class JsonWorkPaperProcessor
{
    internal static WorkPaper DeserializeJsonWorkPaper(string json)
    {
        if (json is null)
        {
            throw new JsonException();
        }

        return JsonSerializer.Deserialize<WorkPaper>(json)!;
    }

    internal static List<WorkPaper> DeserializeJsonWorkPapers(List<string> jsonWorkPapers)
    {
        var workPapers = new List<WorkPaper>();

        foreach (var json in jsonWorkPapers)
        {
            try
            {
                if (json is null)
                {
                    throw new JsonException();
                }

                var workPaper = JsonSerializer.Deserialize<WorkPaper>(json);
                workPapers.Add(workPaper!);
            }
            catch (JsonException exception)
            {
                Log.Fatal("Error deserializing JSON: {message}", exception.Message);
            }
        }

        return workPapers;
    }

    internal static List<WorkPaper> DeserializeJsonWorkPapersParallel(List<string> jsonWorkPapers,
        ParallelOptions parallelOptions, Func<WorkPaper, bool>? filterPredicate = null)
    {
        var concurrentBag = new ConcurrentBag<WorkPaper>();

        Parallel.ForEach(jsonWorkPapers, parallelOptions, json =>
        {
            try
            {
                if (json is null)
                {
                    throw new JsonException();
                }

                var workPaper = JsonSerializer.Deserialize<WorkPaper>(json);

                if (filterPredicate == null || filterPredicate(workPaper!))
                {
                    concurrentBag.Add(workPaper!);
                }
            }
            catch (JsonException exception)
            {
                Log.Fatal("Error deserializing JSON: {message}", exception.Message);
            }
        });

        return concurrentBag.ToList();
    }
}
