using Xunit;

namespace Prometric.Playback.Api.Test
{
    [CollectionDefinition("TestServerFixture")]
    public class TestServerCollection : ICollectionFixture<TestServerFixture>
    {
    }
}
