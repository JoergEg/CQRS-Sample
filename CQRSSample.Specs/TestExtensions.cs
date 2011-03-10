//using System;
//using System.Collections;
//using CQRSSample.Domain.Events;
//using System.Linq;
//using NUnit.Framework;

//namespace CQRSSample.Specs
//{
//    public static class TestExtensions
//    {
//        public static DomainEvent Number(this ICollection events, int value)
//        {
//            return (DomainEvent)events.Cast<object>().ToList()[--value];
//        }

//        public static void CountIs(this ICollection events, int value)
//        {
//            Assert.AreEqual(value, events.Count);
//        }

//        public static void WillBeOfType<TType>(this object theEvent)
//        {
//            Assert.AreEqual(typeof(TType), theEvent.GetType());
//        }

//        public static void WillBe(this object source, object value)
//        {
//            Assert.AreEqual(value, source);
//        }

//        public static void WillNotBe(this object source, object value)
//        {
//            Assert.AreNotEqual(value, source);
//        }

//        public static void WillBeSimuliar(this object source, object value)
//        {
//            Assert.AreEqual(value.ToString(), source.ToString());
//        }

//        public static void WillNotBeSimuliar(this object source, object value)
//        {
//            Assert.AreNotEqual(value.ToString(), source.ToString());
//        }

//        public static void WithMessage(this Exception theException, string message)
//        {
//            Assert.AreEqual(message, theException.Message);
//        }

//        public static TDomainEvent Last<TDomainEvent>(this ICollection events) where TDomainEvent : DomainEvent
//        {
//            return (TDomainEvent)events.Last();
//        }

//        public static object Last(this ICollection events)
//        {
//            return events.Cast<object>().Last();
//        }

//        public static object LastMinus(this ICollection events, int minus)
//        {
//            return events.Cast<object>().ToList()[events.Count - 1 - minus];
//        }

//        //public static TDomainEvent LastMinus<TDomainEvent>(this IEnumerable<DomainEvent> events, int minus)
//        //{
//        //    return (TDomainEvent)events.ToList()[events.Count() - 1 - minus];
//        //}

//        public static TDomainEvent As<TDomainEvent>(this object theObject)
//        {
//            return (TDomainEvent)theObject;
//        }
//    }
//}