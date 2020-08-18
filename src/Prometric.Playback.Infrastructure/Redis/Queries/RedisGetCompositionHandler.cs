using Convey.CQRS.Queries;
using Prometric.Playback.Application.DTO;
using Prometric.Playback.Application.Queries;
using Prometric.Playback.Core.Repositories;
using System.Threading.Tasks;

namespace Prometric.Playback.Infrastructure.Redis.Queries
{
    public class RedisGetCompositionHandler : IQueryHandler<GetComposition, CompositionDto>
    {
        private readonly ICompositionRepository _repository;

        public RedisGetCompositionHandler(ICompositionRepository repository) => _repository = repository;

        public async Task<CompositionDto> HandleAsync(GetComposition query)
        {
            var composition = await _repository.GetAsync(query.CompositionId);
            return composition?.AsDto();
        }
    }
}
