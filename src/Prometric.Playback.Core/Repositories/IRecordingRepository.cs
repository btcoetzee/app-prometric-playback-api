using Prometric.Playback.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Prometric.Playback.Core.Repositories
{
    public interface IRecordingRepository
    {
        Task AddAsync(Recording recording);

        Task<Recording> GetAsync(Guid id);
    }
}
