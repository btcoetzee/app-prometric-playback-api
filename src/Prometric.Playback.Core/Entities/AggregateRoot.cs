namespace Prometric.Playback.Core.Entities
{
    public abstract class AggregateRoot
    {
        public AggregateId Id { get; protected set; }
    }
}
