using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Prometric.Playback.Application.Exceptions;
using Prometric.Playback.Core.Entities;
using Prometric.Playback.Core.Repositories;
using System.Threading.Tasks;

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
            var recording = await _repository.GetAsync(command.RecordingId);
            if (recording is { })
                throw new RecordingAlreadyExistsException(command.RecordingId);

            _log.LogInformation($"Recording {command.RecordingId} created.");

            recording = Recording.Create(command.RecordingId, command.TrackName);

            await _repository.AddAsync(recording);
        }
    }
}
