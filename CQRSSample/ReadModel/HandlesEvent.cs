// ReSharper disable InconsistentNaming

using CQRSSample.Events;

namespace CQRSSample.ReadModel
{
    public interface HandlesEvent<T> where T : IDomainEvent
    {
        void Handle(T domainEvent);
    }
}