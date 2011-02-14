using System;

namespace CQRSSample.Events
{
    public interface IDomainEvent
    {
        Guid AggregateId { get; set; }
        int Version { get; set; }
    }

    [Serializable]
    public class DomainEvent : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
    }
}