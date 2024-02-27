using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class RepresentativeOfficeManager : IRepresentativeOfficeManager
{
    private readonly AppDbContextFactory _dbContextFactory;

    public RepresentativeOfficeManager(AppDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public Result<ICollection<RepresentativeOffice>> TryGetRepresentativesOffice()
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var representativeOffice = dbContext.GetRepresentativeOffices();
        if (representativeOffice is null || representativeOffice.Count == 0)
        {
            var error = new Error($" Not found.", ErrorSeverity.Error);
            return Result<ICollection<RepresentativeOffice>>.NotFound(error);
        }

        return Result<ICollection<RepresentativeOffice>>.Ok(representativeOffice);
    }
}
