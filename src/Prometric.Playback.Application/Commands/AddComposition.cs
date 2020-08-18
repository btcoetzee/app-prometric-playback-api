using Convey.CQRS.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace Prometric.Playback.Application.Commands
{
    public class AddComposition : ICommand
    {
        public AddComposition(Guid compositionId, string compositionName)
        {
            CompositionId = compositionId == Guid.Empty ? Guid.NewGuid() : compositionId;
            CompositionName = compositionName;
        }

        public Guid CompositionId { get; set; }

        [Required]
        public string CompositionName { get; set; }
    }
}
