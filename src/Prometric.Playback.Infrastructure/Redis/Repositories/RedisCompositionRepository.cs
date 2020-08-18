using Microsoft.Extensions.Caching.Distributed;
using Prometric.Playback.Core.Entities;
using Prometric.Playback.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace Prometric.Playback.Infrastructure.Redis.Repositories
{
    public class RedisCompositionRepository : ICompositionRepository
    {
        private readonly IDistributedCache _cache;

        public RedisCompositionRepository(IDistributedCache cache) => _cache = cache;

        public async Task AddAsync(Composition composition)
        => await _cache.SetStringAsync(
            composition.Id.ToString(),
            composition.AsDocument().AsJson());

        public async Task<Composition> GetAsync(Guid id)
        {
            var composition = await _cache.GetStringAsync(id.ToString());
            return composition?.AsCompositionDocument().AsEntity();
        }
    }
}
