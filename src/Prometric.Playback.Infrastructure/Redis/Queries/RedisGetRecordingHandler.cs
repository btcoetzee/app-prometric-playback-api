using Convey.CQRS.Queries;
using Prometric.Playback.Application.DTO;
using Prometric.Playback.Application.Queries;
using Prometric.Playback.Core.Repositories;
using System.Threading.Tasks;
using Prometric.Playback.Application.Exceptions;

namespace Prometric.Playback.Infrastructure.Redis.Queries
{
    public class RedisGetRecordingHandler : IQueryHandler<GetRecording, RecordingDto>
    {
        private readonly IRecordingRepository _repository;

        public RedisGetRecordingHandler(IRecordingRepository repository) => _repository = repository;

        public async Task<RecordingDto> HandleAsync(GetRecording query)
        {
            var recording = await _repository.GetAsync(query.RecordingId);
            return recording?.AsDto();
        }
    }
}
