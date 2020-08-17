using Microsoft.AspNetCore.TestHost;
using System;

namespace Prometric.Playback.Api.Test
{
    public class TestServerFixture : IDisposable
    {
        public TestServer TestServer { get; protected set; }

        public TestServerFixture()
        {
            var hostBuilder = Program.CreateWebHostBuilder(null);
            TestServer = new TestServer(hostBuilder);
        }

        public void Dispose()
        {
            TestServer?.Dispose();
            TestServer = null;
        }
    }
}
