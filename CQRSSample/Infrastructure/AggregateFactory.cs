using System;
using CommonDomain;
using CommonDomain.Persistence;

namespace CQRSSample.Infrastructure
{
    public class AggregateFactory : IConstructAggregates
    {
        public IAggregate Build(Type type, Guid id, IMemento snapshot)
        {
            return Activator.CreateInstance(type, id) as IAggregate; // todo
        }
    }
}