using Convey.CQRS.Commands;

namespace Prometric.Playback.Application.Commands
{
    public class Webhook : ICommand
    {
        public string RoomName { get; set; }
        public string RoomStatus { get; set; }
        public string RoomType { get; set; }
        public string RoomSid { get; set; }
        public string RoomDuration { get; set; }
        public string SequenceNumber { get; set; }
        public string StatusCallbackEvent { get; set; }
        public string Timestamp { get; set; }
        public string AccountSid { get; set; }
    }
}
