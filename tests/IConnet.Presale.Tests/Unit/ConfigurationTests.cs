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
    public void SecurityTokenSettings_Issuer_ShouldBeIConnet.Presale()
    {
        Configuration["SecurityToken:Issuer"]
            .Should()
            .Be("IConnet.Presale");
    }

    [Fact]
    public void SecurityTokenSettings_Audience_ShouldBeIConnet.Presale()
    {
        Configuration["SecurityToken:Audience"]
            .Should()
            .Be("IConnet.Presale");
    }

    [Fact]
    public void UserSecrets_ApiKey_ShouldNotBeNull()
    {
        Configuration["UserSecrets:ApiKey"]
            .Should()
            .NotBeNull();
    }
}
