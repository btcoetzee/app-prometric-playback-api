using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Prometric.Playback.Application.Exceptions;
using Prometric.Playback.Core.Entities;
using Prometric.Playback.Core.Repositories;
using System.Threading.Tasks;

namespace Prometric.Playback.Application.Commands.Handlers
{
    public class AddCompositionHandler : ICommandHandler<AddComposition>
    {
        private readonly ICompositionRepository _repository;
        private readonly ILogger<AddCompositionHandler> _log;
        public AddCompositionHandler(ICompositionRepository repository, ILogger<AddCompositionHandler> log) { 
            _repository = repository;
            _log = log;
        }

        public async Task HandleAsync(AddComposition command)
        {
            var composition = await _repository.GetAsync(command.CompositionId);
            if (composition is { })
                throw new CompositionAlreadyExistsException(command.CompositionId);

            _log.LogInformation($"Composition {command.CompositionId} created.");

            composition = Composition.Create(command.CompositionId, command.CompositionName);

            await _repository.AddAsync(composition);
        }
    }
}
