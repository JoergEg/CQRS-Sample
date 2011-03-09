// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using CQRSSample.Domain.Domain;
using CQRSSample.Domain.Events;
using NUnit.Framework;

namespace CQRSSample.Specs.CustomerSpecs
{
    public class when_non_existing_customer_is_relocating : AggregateRootTestFixture<Customer>
    {
        protected override IEnumerable<DomainEvent> Given()
        {
            yield break;
        }

        protected override void When()
        {
            Sut.RelocateCustomer("Ringstraße", "1a", "1010", "Wien");
        }

        [Test]
        public void should_throw_non_existing_customer_event()
        {
            Assert.IsInstanceOfType(typeof(NonExistingCustomerException), Caught);
        }
    }
}
