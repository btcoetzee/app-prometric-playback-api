using Prometric.Playback.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Prometric.Playback.Core.Repositories
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);

        Task<Book> GetAsync(Guid id);
    }
}
