using Convey.CQRS.Commands;

public class AddRecording : ICommand
    {
        public string MediaUri { get; set; }
        public string RoomSid { get; set; }
        public string RoomName { get; set; }
        public string SourceSid { get; set; }
        public int Size { get; set; }
        public string ParticipantIdentity { get; set; }
        public string RecordingSid { get; set; }
        public int Duration { get; set; }
        public string StatusCallbackEvent { get; set; }
        public System.DateTime Timestamp { get; set; }
        public string AccountSid { get; set; }
        public string RecordingUri { get; set; }
        public string Codec { get; set; }
        public string Container { get; set; }
        public string RoomType { get; set; }
        public string OffsetFromTwilioVideoEpoch { get; set; }
        public string TrackName { get; set; }
        public string ParticipantSid { get; set; }
        public string SequenceNumber { get; set; }
    }