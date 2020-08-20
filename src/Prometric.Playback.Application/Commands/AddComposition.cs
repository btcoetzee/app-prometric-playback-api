using Convey.CQRS.Commands;

namespace Prometric.Playback.Application.Commands
{
    public class AddComposition : ICommand
    {
        public string AccountSid {get; set;}
        public string RoomSid {get; set;}
        public string HookSid {get; set;}
        public string HookUri {get; set;}
        public string HookFriendlyName {get; set;}
        public string CompositionSid {get; set;}
        public string CompositionUri {get; set;}
        public string MediaUri {get; set;}
        public string Duration {get; set;}
        public string Size {get; set;}
        public string PercentageDone {get; set;}
        public string SecondsRemaining {get; set;}
        public string StatusCallbackEvent {get; set;}
        public System.DateTime Timestamp { get; set; }
        public string ExamSessionLabel { get; set; }
    }
}
