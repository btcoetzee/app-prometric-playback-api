using System;
using System.Collections.Concurrent;
using System.Net;
using Convey;
using Convey.WebApi.Exceptions;
using Prometric.Playback.Application.Exceptions;
using Prometric.Playback.Core.Exceptions;

namespace Prometric.Playback.Infrastructure
{
    internal sealed class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        private static readonly ConcurrentDictionary<Type, string> Codes = new ConcurrentDictionary<Type, string>();

        public ExceptionResponse Map(Exception exception)
            => exception switch
            {

                RecordingAlreadyExistsException ex => new ExceptionResponse(new { code = GetCode(ex), reason = ex.Message },
                    HttpStatusCode.Conflict),
                CompositionAlreadyExistsException ex => new ExceptionResponse(new { code = GetCode(ex), reason = ex.Message },
                    HttpStatusCode.Conflict),
                DomainException ex => new ExceptionResponse(new {code = GetCode(ex), reason = ex.Message},
                    HttpStatusCode.BadRequest),
                AppException ex => new ExceptionResponse(new {code = GetCode(ex), reason = ex.Message},
                    HttpStatusCode.BadRequest),
                _ => new ExceptionResponse(new
                    {
                        code = "error", reason = "There was an error. Please, look at the logs for details."
                    },
                    HttpStatusCode.BadRequest)
            };

        private static string GetCode(Exception exception)
        {
            var type = exception.GetType();
            if (Codes.TryGetValue(type, out var code))
            {
                return code;
            }

            var exceptionCode = exception switch
            {
                DomainException domainException when !string.IsNullOrWhiteSpace(domainException.Code) => domainException
                    .Code,
                AppException appException when !string.IsNullOrWhiteSpace(appException.Code) => appException.Code,
                _ => exception.GetType().Name.Underscore().Replace("_exception", string.Empty)
            };

            Codes.TryAdd(type, exceptionCode);
            return exceptionCode;
        }
    }
}