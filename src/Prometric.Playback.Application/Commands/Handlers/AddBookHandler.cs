using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Prometric.Playback.Application.Exceptions;
using Prometric.Playback.Core.Entities;
using Prometric.Playback.Core.Repositories;
using System.Threading.Tasks;

namespace Prometric.Playback.Application.Commands.Handlers
{
    public class AddBookHandler : ICommandHandler<AddBook>
    {
        private readonly IBookRepository _repository;
        private readonly ILogger<AddBookHandler> _log;
        public AddBookHandler(IBookRepository repository, ILogger<AddBookHandler> log) { 
            _repository = repository;
            _log = log;
        }

        public async Task HandleAsync(AddBook command)
        {
            var book = await _repository.GetAsync(command.BookId);
            if (book is { })
                throw new BookAlreadyExistsException(command.BookId);

            _log.LogInformation($"Book {command.BookId} created.");

            book = Book.Create(command.BookId, command.Title);

            await _repository.AddAsync(book);
        }
    }
}
