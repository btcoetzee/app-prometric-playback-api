using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Prometric.Playback.Application.Commands.Handlers;
using System;
using System.Threading.Tasks;
using Twilio.Base;
using Twilio.Rest.Video.V1;

namespace Prometric.Playback.Api.Test
{
    public class TestServerFixture : IDisposable
    {
        public TestServer TestServer { get; protected set; }

        public TestServerFixture()
        {
            var twilio = new Mock<ITwilioService>();

            twilio.Setup(_ => _
                .FetchRecordings(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult<ResourceSet<RecordingResource>>(null));

            var hostBuilder = Program.CreateWebHostBuilder(null);
            hostBuilder.ConfigureServices(s => s
                .AddSingleton<ITwilioService>(twilio.Object));

            TestServer = new TestServer(hostBuilder);
            TestServer.AllowSynchronousIO = true;
        }

        public void Dispose()
        {
            TestServer?.Dispose();
            TestServer = null;
        }
    }
}
