using Convey.CQRS.Queries;
using Prometric.Playback.Application.DTO;
using System;

namespace Prometric.Playback.Application.Queries
{
    public class GetComposition : IQuery<CompositionDto>
    {
        public Guid CompositionId { get; set; }
    }
}
