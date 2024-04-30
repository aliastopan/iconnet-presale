namespace IConnet.Presale.WebApp.Components.Pages;

public class DeveloperPageBase : ComponentBase
{
    [Inject] public AppSettingsService AppSettingsService { get; set; } = default!;
    [Inject] public OptionService OptionService { get; set; } = default!;
    [Inject] public CsvParserService CsvParserService { get; set; } = default!;
    [Inject] public CrmImportService CrmImportService { get; init; } = default!;

    private bool _init = false;

    private const int _byte = 1024;
    private const int _megabyte = 1024 * _byte;

    public int MaxFileSize => 10 * _megabyte;
    public FluentInputFile? FileUploader { get; set; } = default!;
    public int? ProgressPercent { get; set; }
    public string? ProgressTitle { get; set;}

    public FluentInputFileEventArgs[] Files { get; set; } = Array.Empty<FluentInputFileEventArgs>();

    public string? CsvHeader { get; set;}

    public async Task OnCompletedAsync(IEnumerable<FluentInputFileEventArgs> files)
    {
        Files = files.ToArray();
        ProgressPercent = FileUploader!.ProgressPercent;
        ProgressTitle = FileUploader!.ProgressTitle;

        if (Files.Length == 0)
        {
            return;
        }

        FluentInputFileEventArgs fileInput = Files.First();
        FileInfo fileInfo = fileInput.LocalFile!;

        if (fileInput.ContentType != "text/csv")
        {
            LogSwitch.Debug("incorrect file type {0}", fileInput.ContentType);
            return;
        }
        else
        {
            LogSwitch.Debug("file type {0}", fileInput.ContentType);
        }

        bool IsFileCsv = CsvParserService.TryGetCsvFromLocal(fileInfo, out List<string[]>? csv, out var _);

        if (!IsFileCsv || csv is null)
        {
            return;
        }

        LogSwitch.Debug("CSV {0} rows", csv.Count);

        // if (fileInfo.Extension.ToLower() != ".csv")
        // {
        //     LogSwitch.Debug("Incorrect file format of {0}", fileInfo.Extension);

        //     return;
        // }

        // using StreamReader reader = fileInfo.OpenText();

        // string? firstLine = reader.ReadLine();

        // if (firstLine != null)
        // {
        //     CsvHeader = firstLine;
        //     LogSwitch.Debug(firstLine);
        // }
        // else
        // {
        //     LogSwitch.Debug("File is empty.");
        // }


        // delete this later
        foreach (var any in Files)
        {
            any.LocalFile?.Delete();
        }

        await Task.CompletedTask;
    }

    protected override void OnInitialized()
    {
        if (!_init)
        {
            if (AppSettingsService.RootCauseClassifications.Count > 0)
            {
                // foreach (var classification in AppSettingsService.RootCauseClassifications)
                // {
                //     LogSwitch.Debug(classification);
                // }
            }
            else
            {
                // LogSwitch.Debug("ROOT CAUSE CLASSIFICATION IS EMPTY");
            }

            foreach (var option in OptionService.RootCauseOptionStack)
            {
                // LogSwitch.Debug("{0}/{1}", option.rootCause, option.classification);
            }

            _init = true;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await Task.CompletedTask;
    }
}
