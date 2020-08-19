using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prometric.Playback.Api.Test
{
    [Collection("TestServerFixture")]
    public class Webhook
    {
        private readonly HttpClient _httpClient;

        public Webhook(TestServerFixture testServerFixture)
        {
            _httpClient = testServerFixture.TestServer.CreateClient();
        }

        [Fact]
        public async Task OK()
        {
            var content = new StringContent(
"RoomStatus=completed" +
"&RoomType=group-small" +
"&RoomSid=RM5c1e41751a2169607f1cc0d3a9fb1dbb" +
"&RoomName=dea2a67c-4f68-4b77-a7ce-a021f0609780" +
"&RoomDuration=312" +
"&SequenceNumber=1" +
"&StatusCallbackEvent=room-ended" +
"&Timestamp=2020-08-19T17%3A40%3A05.273Z" +
"&AccountSid=AC88c0e76737ac477de3a4700666b63186",
                Encoding.UTF8);

            var response = await _httpClient.PostAsync("webhook", content);

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
