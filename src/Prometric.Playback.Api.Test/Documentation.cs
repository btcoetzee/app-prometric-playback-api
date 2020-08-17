using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Prometric.Playback.Api.Test
{
    [Collection("TestServerFixture")]
    public class Documentation
    {
        private readonly HttpClient _httpClient;

        public Documentation(TestServerFixture testServerFixture)
        {
            _httpClient = testServerFixture.TestServer.CreateClient();
        }

        [Fact]
        public async Task OK()
        {
            var response = await _httpClient.GetAsync(Routes.Swagger);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Contains("swagger-ui", await response.Content.ReadAsStringAsync());
        }
    }
}
