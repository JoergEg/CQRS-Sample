// ReSharper disable InconsistentNaming

using CQRSSample.Events;

namespace CQRSSample.ReadModel
{
    public interface HandlesEvent<in T> where T : IDomainEvent
    {
        void Handle(T domainEvent);
    }
}