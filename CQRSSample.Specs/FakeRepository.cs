using System;
using System.Collections.Generic;
using CommonDomain;
using CommonDomain.Persistence;

namespace CQRSSample.Specs
{
    public class FakeRepository : IRepository
    {
        public IAggregate SavedAggregate { get; set; }

        public TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate
        {
            return Activator.CreateInstance(typeof(TAggregate), id) as TAggregate;
        }

        public void Save(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
        {
            SavedAggregate = aggregate;
        }
    }
}