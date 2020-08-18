using System;
using System.Collections.Generic;
using System.Text;

namespace Prometric.Playback.Application.Exceptions
{
    public class RecordingAlreadyExistsException : AppException
    {
        public override string Code { get; } = "recording_already_exists";

        public RecordingAlreadyExistsException (Guid recordingId) : base($"Recording {recordingId} already exists")
        {
            
        }
    }
}
