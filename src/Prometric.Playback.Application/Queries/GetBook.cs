using Convey.CQRS.Queries;
using Prometric.Playback.Application.DTO;
using System;

namespace Prometric.Playback.Application.Queries
{
    public class GetBook : IQuery<BookDto>
    {
        public Guid BookId { get; set; }
    }
}
