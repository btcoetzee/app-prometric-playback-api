using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Twilio.Rest.Video.V1;
using System.Linq;
using Microsoft.Extensions.Configuration;
using ProProctor.Conferences.Infrastructure.Services;
using Newtonsoft.Json;
using Convey.HTTP;
using Prometric.Playback.Application.Services.Clients;
using System;

namespace Prometric.Playback.Application.Commands.Handlers
{
    public class AddRecordingHandler : ICommandHandler<AddRecording>
    {
        private readonly ILogger<AddRecordingHandler> _log;
        private readonly ITwilioService _service;
        private readonly IExamSessionsClient _examSessionsClient;
        private readonly IConferencesClient _conferencesClient;
        private string _compositionCallbackUri;

        public AddRecordingHandler(ILogger<AddRecordingHandler> log,
            IConfiguration config, ITwilioService service, HttpClientOptions options,
            IExamSessionsClient examSessionsClient,
            IConferencesClient conferencesClient)
        {
            TwilioClientConfig.InitTwilioClient(config);
            _service = service;
            _examSessionsClient = examSessionsClient;
            _conferencesClient = conferencesClient;
            if (options.Services.ContainsKey("composition"))
                _compositionCallbackUri = options.Services["composition"];
        }

        public async Task HandleAsync(AddRecording command)
        {
            // only try to make composite clips when the video recordings are completed.
            // mkv is a type of video
            if (command.Container != "mkv" || command.StatusCallbackEvent != "recording-completed")
                return;

            var examSessionId = Guid.Parse(command.RoomName);
            var examSession = await _examSessionsClient.GetAsync(examSessionId);
            if (examSession == null)
                return;

            var conferenceId = examSession.Conference.Id;
            var participantId = Guid.Parse(command.ParticipantIdentity);
            var participant = await _conferencesClient.GetParticipantAsync(conferenceId, participantId);
            if (participant == null || participant.Role != DTO.ParticipantRole.Candidate)
                return;

            // Fetch relevant recordings for room, participant, and after the current timestamp (adjusted for latency)
            var recordings = await _service.FetchRecordings(
                command.RoomSid, command.ParticipantSid, command.Timestamp);

            if (recordings != null && recordings.Any())
            {
                recordings.ToList().ForEach(r =>
                {
                    _log?.LogInformation($"Fetched recording: {r.Sid} {r.Type} {r.DateCreated}");
                });

                var videoIds = recordings.Where(r => r.Type.Equals(RecordingResource.TypeEnum.Video))
                    .Select(record => record.Sid)
                    .ToList();

                var audioTrackIds = recordings.Where(r => r.Type.Equals(RecordingResource.TypeEnum.Audio))
                    .Select(record => record.Sid)
                    .ToList();

                // Post a new Composition to the Twilio API, use our composition callback
                if (videoIds.Any() && audioTrackIds.Any())
                    await _service.CreateComposition(command.RoomSid, audioTrackIds, videoIds, _compositionCallbackUri);
            }
        }
    }
}
