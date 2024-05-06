using System.Text.Json;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace IConnet.Presale.Infrastructure.Services;

internal class PresaleAppService : IPresaleAppService
{
    private int _settingDbIndex = 0;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly AppSecretSettings _appSecretSettings;

    public PresaleAppService(IConnectionMultiplexer connectionMultiplexer,
        IOptions<AppSecretSettings> appSecretOptions)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _appSecretSettings = appSecretOptions.Value;

        _settingDbIndex = _appSecretSettings.RedisDbIndex + 2;
    }

    public IDatabase DatabaseSetting => _connectionMultiplexer.GetDatabase(_settingDbIndex);

    public async Task<bool> IsNullSettings()
    {
        var key = "PRESALE_APP";
        var result = await GetSettingValueAsync(key);

        return result is null;
    }

    public async Task SetDefaultSettingAsync()
    {
        Log.Warning("Setting up DEFAULT settings.");

        var setting = new PresaleSettingModel
        {
            ChatTemplate = "version_1",
            OfficeHoursPagi = new OfficeHours
            {
                Start = new TimeOnly(8, 0, 0),
                End = new TimeOnly(15, 0, 0)
            },
            OfficeHoursMalam = new OfficeHours
            {
                Start = new TimeOnly(14, 0, 0),
                End = new TimeOnly(21, 0, 0)
            },
            ServiceLevelAgreement = new ServiceLevelAgreement
            {
                Import = TimeSpan.FromMinutes(30),
                PickUp = TimeSpan.FromMinutes(60),
                Validasi = TimeSpan.FromMinutes(10),
                Approval = TimeSpan.FromMinutes(30),
            },
            RootCauseClassification =
            [
                "TIDAK ADA KLASIFIKASI",
                "USER INVALID",
                "KENDALA PENGECEKAN",
                "DOUBLE INPUTAN",
                "JALUR KABEL",
                "JARAK TIDAK TERCOVER",
                "KENDALA FAT",
                "ASSET TERSEGEL",
            ]
        };

        var json = JsonSerializer.Serialize(setting);
        var key = "PRESALE_APP";

        await SetSettingValueAsync(key, json);
    }

    public async Task SetSettingValueAsync(string key, string value, TimeSpan? expiry = null)
    {
        try
        {
            await DatabaseSetting.StringSetAsync(key, value, expiry);
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }

    public async Task<string> GetSettingValueAsync(string key)
    {
        try
        {
            return (await DatabaseSetting.StringGetAsync(key))!;
        }
        catch (TimeoutException exception)
        {
            Log.Fatal($"Redis operation timed out: {exception.Message}");
            throw;
        }
    }
}
