using Convey.CQRS.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace Prometric.Playback.Application.Commands
{
    public class AddBook : ICommand
    {
        public AddBook(Guid bookId, string title)
        {
            BookId = bookId == Guid.Empty ? Guid.NewGuid() : bookId;
            Title = title;
        }

        public Guid BookId { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
