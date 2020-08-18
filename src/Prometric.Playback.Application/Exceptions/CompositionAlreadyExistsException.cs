using System;
using System.Collections.Generic;
using System.Text;

namespace Prometric.Playback.Application.Exceptions
{
    public class CompositionAlreadyExistsException : AppException
    {
        public override string Code { get; } = "composition_already_exists";

        public CompositionAlreadyExistsException (Guid compositionId) : base($"Composition {compositionId} already exists")
        {
            
        }
    }
}
