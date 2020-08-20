using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prometric.Playback.Api.Test
{
    [Collection("TestServerFixture")]
    public class AddRecording
    {
        private readonly HttpClient _httpClient;

        public AddRecording(TestServerFixture testServerFixture)
        {
            _httpClient = testServerFixture.TestServer.CreateClient();
        }

        [Fact]
        public async Task OK()
        {
            var content = new StringContent(
                "MediaUri=%2Fv1%2FRecordings%2FRT2c3f5a84328cb488376b45c221f27998%2FMedia&" + 
                "RoomSid=RM5b9970c25fb6f31a7da94fe683be0957&" + 
                "RoomName=1f03d8d2-4837-4c99-b1cf-e279ba097b17&" + 
                "SourceSid=MT863e43b23ad81723cc410f548045c6e3&" + 
                "Size=105956&" + 
                "ParticipantIdentity=ad9d1899-0a48-4cc5-a601-da0b33e4d726&" + 
                "RecordingSid=RT2c3f5a84328cb488376b45c221f27998&" + 
                "Duration=3&" + 
                "StatusCallbackEvent=recording-completed&" + 
                "Timestamp=2020-08-20T01%3A42%3A05.839174Z&" + 
                "AccountSid=AC88c0e76737ac477de3a4700666b63186&" + 
                "RecordingUri=%2Fv1%2FRecordings%2FRT2c3f5a84328cb488376b45c221f27998&" + 
                "Codec=vp8&" + 
                "Container=mkv&" + 
                "RoomType=group-small&" + 
                "OffsetFromTwilioVideoEpoch=155137307765&" + 
                "TrackName=6ec82db2-390d-47d2-aec8-0e6606f15c51&" + 
                "ParticipantSid=PAec462533b0a659b7eb122a7ff9e2312e",
                Encoding.UTF8);

            var response = await _httpClient.PostAsync(Routes.Recordings, content);

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
