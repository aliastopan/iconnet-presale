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

    internal static IEnumerable<WorkPaper> DeserializeJsonWorkPapersParallel(IEnumerable<string> jsonWorkPapers,
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
