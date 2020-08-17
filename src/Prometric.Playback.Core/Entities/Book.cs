using System;
using System.Threading.Tasks;

namespace Prometric.Playback.Core.Entities
{
    public class Book : AggregateRoot
    {
        public Book(AggregateId id, string title)
        {
            Id = id;
            Title = title;
        }

        public static Book Create(Guid bookId, string title)
        {
            return new Book(bookId, title);            
        }

        public string Title { get; set; }
    }
}
