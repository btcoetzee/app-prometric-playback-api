using System;
using System.Threading.Tasks;

namespace Prometric.Playback.Core.Entities
{
    public class Composition : AggregateRoot
    {
        public Composition(AggregateId id, string compositionName)
        {
            Id = id;
            CompositionName = compositionName;
        }

        public static Composition Create(Guid compositionId, string compositionName)
        {
            return new Composition(compositionId, compositionName);            
        }

        public string CompositionName { get; set; }
    }
}
