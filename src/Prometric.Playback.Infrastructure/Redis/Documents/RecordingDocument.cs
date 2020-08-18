using System;

namespace Prometric.Playback.Infrastructure.Redis.Documents
{
    public class RecordingDocument
    {
        public Guid Id { get; set; }

        public string TrackName { get; set; }
    }
}
