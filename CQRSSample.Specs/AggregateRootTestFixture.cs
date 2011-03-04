// ReSharper disable InconsistentNaming
using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Core;
using CommonDomain;
using CQRSSample.Events;
using NUnit.Framework;

namespace CQRSSample.Specs
{
    [TestFixture]
    public abstract class AggregateRootTestFixture<T> where T : IAggregate, new()
    {
        protected Exception Caught;
        protected T Sut;
        protected ICollection Events;

        protected abstract IEnumerable<DomainEvent> Given();
        protected abstract void When();

        [SetUp]
        public void Setup()
        {
            Sut = new T();
            Given().ForEach(x => Sut.ApplyEvent(x));

            try
            {
                When();
                Events = Sut.GetUncommittedEvents();
            }
            catch (Exception ex)
            {
                Caught = ex;
            }
        }
    }
}