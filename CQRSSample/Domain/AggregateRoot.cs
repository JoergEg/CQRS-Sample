using System;
using System.Collections.Generic;
using System.Reflection;
using CQRSSample.Events;

namespace CQRSSample.Domain
{
    public abstract class AggregateRoot
    {
        private readonly List<DomainEvent> _changes = new List<DomainEvent>();

        public abstract Guid Id { get; }
        public int Version { get; internal set; }

        public IEnumerable<DomainEvent> GetChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadFromHistory(IEnumerable<DomainEvent> history)
        {
            foreach (var e in history) ApplyChange(e, false);
        }

        protected void ApplyChange(DomainEvent domainEvent)
        {
            ApplyChange(domainEvent, true);
        }

        private void ApplyChange(DomainEvent domainEvent, bool isNew)
        {
            //TODO: .Net Framework 4 -> dynamic Feature
            //this.AsDynamic().Apply(domainEvent);

            if (domainEvent == null)
                throw new ArgumentNullException("domainEvent");
            var eventTypeName = domainEvent.GetType().Name;
            var suffixIndex = eventTypeName.LastIndexOf("Event");
            if (suffixIndex <= 0)
                throw new InvalidOperationException("Invalid event name: " + eventTypeName);

            const string methodName = "Apply";
            var methodInfo = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new[]{domainEvent.GetType()}, null);
            methodInfo.Invoke(this, new[] { domainEvent });

            if (isNew) 
                _changes.Add(domainEvent);
        }
    }
}