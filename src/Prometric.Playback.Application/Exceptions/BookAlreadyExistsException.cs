using System;
using System.Collections.Generic;
using System.Text;

namespace Prometric.Playback.Application.Exceptions
{
    public class BookAlreadyExistsException : AppException
    {
        public override string Code { get; } = "book_already_exists";

        public BookAlreadyExistsException (Guid bookId) : base($"Book {bookId} already exists")
        {
            
        }
    }
}
