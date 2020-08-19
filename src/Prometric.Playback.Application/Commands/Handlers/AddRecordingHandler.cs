using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Prometric.Playback.Application.Exceptions;
using Prometric.Playback.Core.Entities;
using Prometric.Playback.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Video.V1.Room;

namespace Prometric.Playback.Application.Commands.Handlers
{
    public class AddRecordingHandler : ICommandHandler<AddRecording>
    {
        private readonly IRecordingRepository _repository;
        private readonly ILogger<AddRecordingHandler> _log;
        public AddRecordingHandler(IRecordingRepository repository, ILogger<AddRecordingHandler> log) { 
            _repository = repository;
            _log = log;
        }

        public async Task HandleAsync(AddRecording command)
        {
            // TODO: move to environment variables
            TwilioClient.Init("ACCT_SID", "AUTH_TOKEN");
            if (command.RecordingStatusCallback.StatusCallbackEvent == "recording-completed" && command.RecordingStatusCallback.ParticipantIdentity == "candidate") {
                // Fetch all recordings for the given participant and room sid
                var recordings = await RoomRecordingResource.ReadAsync(pathRoomSid: command.RecordingStatusCallback.RoomSid, sourceSid: command.RecordingStatusCallback.ParticipantSid);

                foreach(var record in recordings) {
                    Console.WriteLine(record.Sid);
                }
            }
        }
    }
}
