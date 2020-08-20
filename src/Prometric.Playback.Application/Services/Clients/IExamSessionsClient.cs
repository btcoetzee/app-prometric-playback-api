using Prometric.Playback.Application.DTO;
using System;
using System.Threading.Tasks;

namespace Prometric.Playback.Application.Services.Clients
{
    public interface IExamSessionsClient
    {
        Task<ExamSessionDto> GetAsync(Guid examSessionId);
    }
}