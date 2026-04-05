using Application.Mappings;

namespace Tests;

[SetUpFixture]
public sealed class MapsterTestSetup
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        MapsterConfiguration.Register();
    }
}
