using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Prometric.Playback.Application.Commands.Handlers;
using Prometric.Playback.Application.DTO;
using Prometric.Playback.Application.Services.Clients;
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

            var examSession = new ExamSessionDto() {
                Conference = new ConferenceDto() {
                    Id = Guid.NewGuid()
                }
            };
            var examSessions = new Mock<IExamSessionsClient>();
            examSessions.Setup(_ => _.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(examSession));

            var participant = new ParticipantDto() {
                Role = ParticipantRole.Candidate
            };
            var conferences = new Mock<IConferencesClient>();
            conferences.Setup(_ => _.GetParticipantAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(participant));

            var hostBuilder = Program.CreateWebHostBuilder(null);
            hostBuilder.ConfigureServices(s => s
                .AddSingleton(twilio.Object)
                .AddSingleton(examSessions.Object)
                .AddSingleton(conferences.Object)
                );

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
