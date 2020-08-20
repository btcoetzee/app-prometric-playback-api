using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Prometric.Playback.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
<<<<<<< HEAD
=======
using Twilio;
>>>>>>> 544b964da98a024b2540cf31b57df15e5f4ebe84
using Twilio.Rest.Video.V1;
using System.Linq;
using static Twilio.Rest.Video.V1.CompositionResource;
using Microsoft.Extensions.Configuration;
using ProProctor.Conferences.Infrastructure.Services;

namespace Prometric.Playback.Application.Commands.Handlers
{
    public class AddRecordingHandler : ICommandHandler<AddRecording>
    {
        private readonly IRecordingRepository _repository;
        private readonly ILogger<AddRecordingHandler> _log;
        public AddRecordingHandler(IRecordingRepository repository, ILogger<AddRecordingHandler> log, IConfiguration config) { 
            TwilioClientConfig.InitTwilioClient(config);
            _repository = repository;
            _log = log;
        }

        public async Task HandleAsync(AddRecording command)
        {
            var groupingSid = new List<string> {
                command.RoomSid, command.ParticipantSid
            };

            // Fetch relevant recordings for room, participant, and after the current timestamp (adjusted for latency)
            var recordings = await RecordingResource.ReadAsync(groupingSid: groupingSid, dateCreatedAfter: command.Timestamp.AddMinutes(-1));
            // Post a new Composition to the Twilio API, use our composition callback
            var layout = new { single = new { video_sources = recordings.Where(record => record.Type.ToString() == "video").Select(record => record.Sid).ToList() }};
            var composition = await CompositionResource.CreateAsync(
                roomSid: command.RoomSid,
                audioSources: recordings.Where(record => record.Type.ToString() == "audio").Select(record => record.Sid).ToList(),
                videoLayout: layout,
                statusCallback: new Uri("REPLACE_ME"),
                format: FormatEnum.Mp4
            );
        }
    }
}
