using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.Base;
using Twilio.Rest.Video.V1;

namespace Prometric.Playback.Application.Commands.Handlers
{
    public interface ITwilioService
    {
        Task CreateComposition(string roomSid, List<string> audioTrackIds, List<string> videoIds, string compositionCallbackUri);
        Task<ResourceSet<RecordingResource>> FetchRecordings(string roomSid, string participantSid, DateTime timestamp);
    }
}