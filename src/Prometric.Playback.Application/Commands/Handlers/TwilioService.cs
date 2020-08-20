using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.Rest.Video.V1;

namespace Prometric.Playback.Application.Commands.Handlers
{
    public class TwilioService : ITwilioService
    {        
        public async Task<Twilio.Base.ResourceSet<RecordingResource>> FetchRecordings(string roomSid, string participantSid, DateTime timestamp)
            => await RecordingResource.ReadAsync(
                groupingSid: new List<string> {
                    roomSid,
                    participantSid
                },
                dateCreatedAfter: timestamp.AddMinutes(-1));

        public async Task CreateComposition(string roomSid, List<string> audioTrackIds, List<string> videoIds, string compositionCallbackUri)
            => await CompositionResource.CreateAsync(
                roomSid: roomSid,
                audioSources: audioTrackIds,
                videoLayout: new { single = new { video_sources = videoIds } },
                statusCallback: new Uri(compositionCallbackUri),
                format: CompositionResource.FormatEnum.Mp4
            );
    }
}
