using Prometric.Playback.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Prometric.Playback.Core.Repositories
{
    public interface ICompositionRepository
    {
        Task AddAsync(Composition composition);

        Task<Composition> GetAsync(Guid id);
    }
}
