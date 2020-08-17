using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Prometric.Playback.Api.Test
{
    [Collection("TestServerFixture")]
    public class Metrics
    {
        private readonly HttpClient _httpClient;

        public Metrics(TestServerFixture testServerFixture)
        {
            _httpClient = testServerFixture.TestServer.CreateClient();
        }

        [Fact]
        public async Task OK()
        {
            var response = await _httpClient.GetAsync(Routes.Metrics);

            Assert.True(response.IsSuccessStatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("# HELP application_httprequests_transactions", content);
        }
    }
}
