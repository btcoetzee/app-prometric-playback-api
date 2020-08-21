using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prometric.Playback.Api.Test
{
    [Collection("TestServerFixture")]
    public class AddComposition
    {
        private readonly HttpClient _httpClient;

        public AddComposition(TestServerFixture testServerFixture)
        {
            _httpClient = testServerFixture.TestServer.CreateClient();
        }

        [Fact]
        public async Task OK()
        {
            var content = new StringContent(
                "MediaUri=%2Fv1%2FCompositions%2FCJ7b1065c2ecd57deb528aab74fa6d57a2%2FMedia&" + 
                "RoomSid=RM5b9970c25fb6f31a7da94fe683be0957&" +
                "Size=35199906&" +
                "CompositionSid=CJ7b1065c2ecd57deb528aab74fa6d57a2&" +
                "Duration=192&" +
                "StatusCallbackEvent=composition-available&" + 
                "Timestamp=2020-08-20T01%3A42%3A05.839174Z&" +
                "AccountSid=ACc0ebac02400d7398f5d726bc8f04ddc1&" +
                "CompositionUri=%2Fv1%2FCompositions%2FCJ7b1065c2ecd57deb528aab74fa6d57a2",
                Encoding.UTF8);
            var response = await _httpClient.PostAsync(Routes.Composition, content);

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
