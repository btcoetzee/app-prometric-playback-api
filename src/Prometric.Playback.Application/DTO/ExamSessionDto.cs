using System;

namespace Prometric.Playback.Application.DTO
{
    public class ExamSessionDto
    {
        public Guid Id { get; set; }

        public ConferenceDto Conference { get; set; }
    }
}