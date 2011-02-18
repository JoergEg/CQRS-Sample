using System;
using CQRSSample.Domain;

namespace CQRSSample.Specs
{
    public class FakeRepository<T> : IRepository<T> where T : AggregateRoot, new()
    {
        public T SavedAggregate { get; set; }

        public void Save(T aggregate, int expectedVersion)
        {
            SavedAggregate = aggregate;
        }

        public T GetById(Guid id)
        {
            return new T();
        }
    }
}