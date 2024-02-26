using IConnet.Presale.Application.Common.Interfaces.Services;

namespace IConnet.Presale.Api.Services;

public class RefreshTokenCollectorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PeriodicTimer _periodicTimer;

    public RefreshTokenCollectorService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _periodicTimer = new PeriodicTimer(TimeSpan.FromDays(7));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _periodicTimer.WaitForNextTickAsync(stoppingToken)
            && !stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();

            var refreshTokenService = scope.ServiceProvider.GetRequiredService<IRefreshTokenService>();
            var count = await refreshTokenService.DeleteUsedRefreshTokensAsync(daysBefore: 3);

            LogSwitch.Debug("Collecting refresh tokens {count}", count);
        }
    }
}
