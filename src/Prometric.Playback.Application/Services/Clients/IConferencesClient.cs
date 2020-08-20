using Prometric.Playback.Application.DTO;
using System;
using System.Threading.Tasks;

namespace Prometric.Playback.Application.Services.Clients
{
    public interface IConferencesClient
    {
        Task<ParticipantDto> GetParticipantAsync(Guid conferenceId, Guid participantId);
    }
}