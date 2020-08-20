using Convey.HTTP;
using Prometric.Playback.Application.DTO;
using Prometric.Playback.Application.Services.Clients;
using System;
using System.Threading.Tasks;

namespace Prometric.Playback.Infrastructure.Services.Clients
{
    public class ConferencesClient : IConferencesClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;

        public ConferencesClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["conferences"];
        }

        public Task<ParticipantDto> GetParticipantAsync(Guid conferenceId, Guid participantId)
            => _httpClient.GetAsync<ParticipantDto>(
                    $"{_url}/conferences/{conferenceId}/participants/{participantId}");
    }
}
