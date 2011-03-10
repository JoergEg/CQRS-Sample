//// ReSharper disable InconsistentNaming
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using CommonDomain;
//using CQRSSample.Domain.Events;
//using NUnit.Framework;

//namespace CQRSSample.Specs
//{
//    [TestFixture]
//    public abstract class AggregateRootTestFixture<T> where T : IAggregate, new()
//    {
//        protected Exception Caught;
//        protected T Sut;
//        protected ICollection Events;

//        protected abstract IEnumerable<DomainEvent> Given();
//        protected abstract void When();

//        [SetUp]
//        public void Setup()
//        {
//            Sut = new T();
//            foreach (var @event in this.Given())
//            {
//                Sut.ApplyEvent(@event);
//            }

//            try
//            {
//                When();
//                Events = Sut.GetUncommittedEvents();
//            }
//            catch (Exception ex)
//            {
//                Caught = ex;
//            }
//        }
//    }
//}