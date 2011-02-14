using System.Collections.Generic;
using CQRSSample.Events;

namespace CQRSSample.Domain
{
    public interface IAggregateRoot
    {
        IEnumerable<DomainEvent> GetChanges();
        void LoadFromHistory(IEnumerable<DomainEvent> history);
    }
}