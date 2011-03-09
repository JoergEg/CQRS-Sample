using System;

namespace CQRSSample.Domain.Events
{
    //public interface IDomainEvent
    //{
    //    Guid AggregateId { get; set; }
    //    int Version { get; set; }
    //}

    [Serializable]
    public class DomainEvent
    {
        public Guid AggregateId { get; set; }
        //public int Version { get; set; }
    }
}