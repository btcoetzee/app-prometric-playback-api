using System;
using System.ComponentModel.DataAnnotations;
using Convey.CQRS.Commands;
using Prometric.Playback.Application.Entities;

public class AddRecording : ICommand
    {
        public AddRecording(IRecordingStatusCallback recordingStatusCallback)
        {
            RecordingStatusCallback = recordingStatusCallback ?? throw new ArgumentNullException(nameof(recordingStatusCallback));
        }
        [Required]
        public IRecordingStatusCallback RecordingStatusCallback { get; set; }
    }