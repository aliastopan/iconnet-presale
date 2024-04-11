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
    public ICollection<string> DirectApprovalOptions { get; init; } = [];
    public ICollection<string> KantorPerwakilanOptions { get; init; } = [];
    public ICollection<string> RootCauseOptions { get; init; } = [];

    public void PopulateDirectApproval(ICollection<DirectApprovalDto> directApprovalDto)
    {
        DirectApprovalOptions.Clear();

        foreach (var dto in directApprovalDto)
        {
            DirectApprovalOptions.Add(dto.Description);
        }
    }

    public void PopulateKantorPerwakilanOptions(ICollection<RepresentativeOfficeDto> representativeOfficeDtos)
    {
        KantorPerwakilanOptions.Clear();

        foreach (var dto in representativeOfficeDtos)
        {
            KantorPerwakilanOptions.Add(dto.Perwakilan);
        }
    }

    public void PopulateRootCauseOptions(ICollection<RootCausesDto> rootCauseDto)
    {
        RootCauseOptions.Clear();

        foreach (var dto in rootCauseDto)
        {
            RootCauseOptions.Add(dto.Cause);
        }
    }
}
