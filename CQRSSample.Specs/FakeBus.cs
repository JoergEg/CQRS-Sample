using System.Collections.Generic;
using CQRSSample.Events;

namespace CQRSSample.Specs
{
    //public class FakeBus : IPublishMessages
    //{
    //    private readonly ICollection<DomainEvent> _domainEvents;

    //    public FakeBus(ICollection<DomainEvent> domainEvents)
    //    {
    //        _domainEvents = domainEvents;
    //    }

    //    public void Dispose()
    //    {
    //    }

    //    public void Publish(Commit commit)
    //    {
    //        commit.Events.ToList().ForEach(e => _domainEvents.Add(e.Body as DomainEvent));
    //    }
    //}
}