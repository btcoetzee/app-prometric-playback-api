using Convey.CQRS.Queries;
using Prometric.Playback.Application.DTO;
using Prometric.Playback.Application.Queries;
using Prometric.Playback.Core.Repositories;
using System.Threading.Tasks;
using Prometric.Playback.Application.Exceptions;

namespace Prometric.Playback.Infrastructure.Redis.Queries
{
    public class RedisGetBookHandler : IQueryHandler<GetBook, BookDto>
    {
        private readonly IBookRepository _repository;

        public RedisGetBookHandler(IBookRepository repository) => _repository = repository;

        public async Task<BookDto> HandleAsync(GetBook query)
        {
            var book = await _repository.GetAsync(query.BookId);
            return book?.AsDto();
        }
    }
}
