using Convey.CQRS.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace Prometric.Playback.Application.Commands
{
    public class AddRecording : ICommand
    {
        public AddRecording(Guid recordingId, string trackName)
        {
            RecordingId = recordingId == Guid.Empty ? Guid.NewGuid() : recordingId;
            TrackName = trackName;
        }

        public Guid RecordingId { get; set; }

        [Required]
        public string TrackName { get; set; }
    }
}
