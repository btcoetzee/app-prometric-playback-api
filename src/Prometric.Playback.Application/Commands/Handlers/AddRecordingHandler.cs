using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Twilio.Rest.Video.V1;
using System.Linq;
using Microsoft.Extensions.Configuration;
using ProProctor.Conferences.Infrastructure.Services;
using System.Resources;

namespace Prometric.Playback.Application.Commands.Handlers
{
    public class AddRecordingHandler : ICommandHandler<AddRecording>
    {
        private readonly ILogger<AddRecordingHandler> _log;
        private readonly ITwilioService _service;

        public AddRecordingHandler(ILogger<AddRecordingHandler> log, 
            IConfiguration config, ITwilioService service)
        {
            TwilioClientConfig.InitTwilioClient(config);
            _service = service;
        }

        public async Task HandleAsync(AddRecording command)
        {
            // Fetch relevant recordings for room, participant, and after the current timestamp (adjusted for latency)
            var recordings = await _service.FetchRecordings(
                command.RoomSid, command.ParticipantSid, command.Timestamp);

            if (recordings != null)
            {
                var videoIds = recordings.Where(r => r.Type.Equals(RecordingResource.TypeEnum.Video))
                    .Select(record => record.Sid)
                    .ToList();

                var audioTrackIds = recordings.Where(r => r.Type.Equals(RecordingResource.TypeEnum.Audio))
                    .Select(record => record.Sid)
                    .ToList();

                // Post a new Composition to the Twilio API, use our composition callback
                await _service.CreateComposition(command.RoomSid, audioTrackIds, videoIds);
            }
        }
    }
}
