namespace IConnet.Presale.Application.Common.Interfaces.Persistence;

public interface IAppDbContextSeeder
{
    Task<int> GenerateUsersAsync();
}
