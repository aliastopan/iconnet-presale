using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Services;

public class OptionService
{
    public OptionService()
    {
        SearchFilterOptions =
        [
            "ID PERMOHONAN"
        ];
    }

    public ICollection<string> SearchFilterOptions { get ; init; } = [];
    public ICollection<string> KantorPerwakilanOptions { get; init; } = [];
    public ICollection<string> RootCauseOptions { get; init; } = [];

    public void PopulateKantorPerwakilanOptions(ICollection<RepresentativeOfficeDto> representativeOfficeDtos)
    {
        foreach (var dto in representativeOfficeDtos)
        {
            KantorPerwakilanOptions.Add(dto.Perwakilan);
        }
    }

    public void PopulateRootCauseOptions(ICollection<RootCausesDto> rootCauseDto)
    {
        foreach (var dto in rootCauseDto)
        {
            RootCauseOptions.Add(dto.Cause);
        }
    }
}
