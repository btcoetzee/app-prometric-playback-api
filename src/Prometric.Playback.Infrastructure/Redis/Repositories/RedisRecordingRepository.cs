using Microsoft.Extensions.Caching.Distributed;
using Prometric.Playback.Core.Entities;
using Prometric.Playback.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace Prometric.Playback.Infrastructure.Redis.Repositories
{
    public class RedisRecordingRepository : IRecordingRepository
    {
        private readonly IDistributedCache _cache;

        public RedisRecordingRepository(IDistributedCache cache) => _cache = cache;

        public async Task AddAsync(Recording recording)
        => await _cache.SetStringAsync(
            recording.Id.ToString(),
            recording.AsDocument().AsJson());

        public async Task<Recording> GetAsync(Guid id)
        {
            var recording = await _cache.GetStringAsync(id.ToString());
            return recording?.AsRecordingDocument().AsEntity();
        }
    }
}
