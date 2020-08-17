using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Prometric.Playback.Api.Test
{
    [Collection("TestServerFixture")]
    public class Default
    {
        private readonly HttpClient _httpClient;

        public Default(TestServerFixture testServerFixture)
        {
            _httpClient = testServerFixture.TestServer.CreateClient();
        }

        [Fact]
        public async Task OK()
        {
            var response = await _httpClient.GetAsync(Routes.Default);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("Prometric.Playback v1", await response.Content.ReadAsStringAsync());
        }
    }
}
