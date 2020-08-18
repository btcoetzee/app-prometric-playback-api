using Convey.CQRS.Queries;
using Prometric.Playback.Application.DTO;
using System;

namespace Prometric.Playback.Application.Queries
{
    public class GetRecording : IQuery<RecordingDto>
    {
        public Guid RecordingId { get; set; }
    }
}
