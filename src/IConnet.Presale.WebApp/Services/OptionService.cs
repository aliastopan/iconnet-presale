using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Services;

public class OptionService
{
    public ICollection<string> KantorPerwakilanOptions { get; init; } = [];

    public void PopulateKantorPerwakilan(ICollection<RepresentativeOfficeDto> representativeOfficeDtos)
    {
        foreach (var dto in representativeOfficeDtos)
        {
            KantorPerwakilanOptions.Add(dto.Perwakilan);
        }

        LogSwitch.Debug("Representative Office {count}: ", KantorPerwakilanOptions.Count);
    }
}
