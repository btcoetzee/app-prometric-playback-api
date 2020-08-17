using System;

namespace Prometric.Playback.Infrastructure.Redis.Documents
{
    public class BookDocument
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
    }
}
