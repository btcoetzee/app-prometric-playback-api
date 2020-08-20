using System;

namespace Prometric.Playback.Application.DTO
{

    public class ParticipantDto
    {
        public Guid Id { get; set; }

        public ParticipantRole Role { get; set; }
    }

    public enum ParticipantRole
    {
        Candidate,
        Readiness,
        Proctor,
        Security
    }
}