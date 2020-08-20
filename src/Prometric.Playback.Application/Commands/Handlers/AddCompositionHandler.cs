using Convey.CQRS.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Prometric.Playback.Core.Repositories;
using ProProctor.Conferences.Infrastructure.Services;
using System.Threading.Tasks;

namespace Prometric.Playback.Application.Commands.Handlers
{
    public class AddCompositionHandler : ICommandHandler<AddComposition>
    {
        private readonly ICompositionRepository _repository;
        private readonly ILogger<AddCompositionHandler> _log;
        public AddCompositionHandler(ICompositionRepository repository, ILogger<AddCompositionHandler> log, IConfiguration config) { 
            TwilioClientConfig.InitTwilioClient(config);
            _repository = repository;
            _log = log;
        }

        public async Task HandleAsync(AddComposition command)
        {
            // Kyle do work here
        }
    }
}
