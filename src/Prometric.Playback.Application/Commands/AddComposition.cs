using Convey.CQRS.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace Prometric.Playback.Application.Commands
{
    public class AddComposition : ICommand
    {
        public AddComposition(Guid compositionId, string trackName)
        {
            CompositionId = compositionId == Guid.Empty ? Guid.NewGuid() : compositionId;
            CompositionName = trackName;
        }

        public Guid CompositionId { get; set; }

        [Required]
        public string CompositionName { get; set; }
    }
}
