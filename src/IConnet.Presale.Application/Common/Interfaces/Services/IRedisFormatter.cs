namespace IConnet.Presale.Application.Common.Interfaces.Services;

public interface IRedisFormatter
{
    Task<int> ReformatArchiveAsync();
}
