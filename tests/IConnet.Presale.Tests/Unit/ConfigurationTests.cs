namespace IConnet.Presale.Tests.Unit;

public class ConfigurationTests : UnitTest
{
    [Fact]
    public void Configuration_ShouldUseInMemoryDatabase()
    {
        Configuration.GetValue<bool>("UseInMemoryDatabase")
            .Should()
            .BeTrue();
    }

    [Fact]
    public void SecurityTokenSettings_Issuer_ShouldBeIConnetPresale()
    {
        Configuration["AppSecrets:JwtIssuer"]
            .Should()
            .Be("CleanArch");
    }

    [Fact]
    public void SecurityTokenSettings_Audience_ShouldBeIConnetPresale()
    {
        Configuration["AppSecrets:JwtAudience"]
            .Should()
            .Be("CleanArch");
    }

    [Fact]
    public void UserSecrets_MasterKey_ShouldNotBeNull()
    {
        Configuration["AppSecrets:MasterKey"]
            .Should()
            .NotBeNull();
    }
}
