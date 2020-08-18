using System;
using System.Threading.Tasks;

namespace Prometric.Playback.Core.Entities
{
    public class Recording : AggregateRoot
    {
        public Recording(AggregateId id, string trackName)
        {
            Id = id;
            TrackName = trackName;
        }

        public static Recording Create(Guid recordingId, string trackName)
        {
            return new Recording(recordingId, trackName);            
        }

        public string TrackName { get; set; }
    }
}
