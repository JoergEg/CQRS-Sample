// ReSharper disable InconsistentNaming
using System;
using System.Collections.Generic;
using CQRSSample.Domain;
using CQRSSample.Events;
using NUnit.Framework;

namespace CQRSSample.Specs
{
    [TestFixture]
    public abstract class AggregateRootTestFixture<T> where T : AggregateRoot, new()
    {
        protected Exception Caught;
        protected T Sut;
        protected List<DomainEvent> Events;

        protected abstract IEnumerable<DomainEvent> Given();
        protected abstract void When();

        [SetUp]
        public void Setup()
        {
            Sut = new T();
            Sut.LoadFromHistory(Given());

            try
            {
                When();
                Events = new List<DomainEvent>(Sut.GetChanges());
            }
            catch (Exception ex)
            {
                Caught = ex;
            }
        }
    }
}