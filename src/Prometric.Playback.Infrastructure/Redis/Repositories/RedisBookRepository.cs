using Microsoft.Extensions.Caching.Distributed;
using Prometric.Playback.Core.Entities;
using Prometric.Playback.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace Prometric.Playback.Infrastructure.Redis.Repositories
{
    public class RedisBookRepository : IBookRepository
    {
        private readonly IDistributedCache _cache;

        public RedisBookRepository(IDistributedCache cache) => _cache = cache;

        public async Task AddAsync(Book book)
        => await _cache.SetStringAsync(
            book.Id.ToString(),
            book.AsDocument().AsJson());

        public async Task<Book> GetAsync(Guid id)
        {
            var book = await _cache.GetStringAsync(id.ToString());
            return book?.AsDocument().AsEntity();
        }
    }
}
