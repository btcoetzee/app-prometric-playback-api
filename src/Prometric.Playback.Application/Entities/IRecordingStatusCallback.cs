namespace Prometric.Playback.Application.Entities
{
    public interface IRecordingStatusCallback
    {
        string MediaUri { get; set; }
        string RoomSid { get; set; }
        string RoomName { get; set; }
        string SourceSid { get; set; }
        int Size { get; set; }
        string ParticipantIdentity { get; set; }
        string RecordingSid { get; set; }
        int Duration { get; set; }
        string StatusCallbackEvent { get; set; }
        string Timestamp { get; set; }
        string AccountSid { get; set; }
        string RecordingUri { get; set; }
        string Codec { get; set; }
        string Container { get; set; }
        string RoomType { get; set; }
        string OffsetFromTwilioVideoEpoch { get; set; }
        string TrackName { get; set; }
        string ParticipantSid { get; set; }
    }
}