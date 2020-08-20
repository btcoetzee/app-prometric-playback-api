using Convey.HTTP;
using Prometric.Playback.Application.DTO;
using Prometric.Playback.Application.Services.Clients;
using System;
using System.Threading.Tasks;

namespace Prometric.Playback.Infrastructure.Services.Clients
{
    public class ExamSessionsClient : IExamSessionsClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;

        public ExamSessionsClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["exam-sessions"];
        }

        public Task<ExamSessionDto> GetAsync(Guid examSessionId)
            => _httpClient.GetAsync<ExamSessionDto>($"{_url}/exam-sessions/{examSessionId}");
    }
}
