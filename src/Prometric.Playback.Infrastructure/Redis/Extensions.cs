using Newtonsoft.Json;
using Prometric.Playback.Application.DTO;
using Prometric.Playback.Core.Entities;
using Prometric.Playback.Infrastructure.Redis.Documents;

namespace Prometric.Playback.Infrastructure.Redis
{
    public static class Extensions
    {
        public static string AsJson(this BookDocument doc)
            => JsonConvert.SerializeObject(doc);

        public static BookDocument AsDocument(this Book entity)
            => new BookDocument {
                Id = entity.Id,
                Title = entity.Title
            };

        public static BookDocument AsDocument(this string json)
            => JsonConvert.DeserializeObject<BookDocument>(json);

        public static Book AsEntity(this BookDocument doc)
            => new Book(doc.Id, doc.Title);

        public static BookDto AsDto(this Book entity)
            => new BookDto()
            {
                BookId = entity.Id,
                Title = entity.Title
            };
    }
}
