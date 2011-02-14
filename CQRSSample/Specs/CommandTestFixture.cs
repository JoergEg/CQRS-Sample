using System;
using System.Collections.Generic;
using CQRSSample.CommandHandlers;
using CQRSSample.Commands;
using CQRSSample.Domain;
using CQRSSample.Events;
using NUnit.Framework;

namespace CQRSSample.Specs
{
    [TestFixture]
    public abstract class CommandTestFixture<TCommand, TCommandHandler, TAggregateRoot>
        where TCommand : Command
        where TCommandHandler : class, Handles<TCommand>
        where TAggregateRoot : AggregateRoot, new()
    {
        protected TAggregateRoot AggregateRoot;
        protected Handles<TCommand> CommandHandler;
        protected Exception CaughtException;
        protected IEnumerable<DomainEvent> PublishedEvents;
        protected FakeRepository<TAggregateRoot> Repository;
        protected virtual void SetupDependencies() { }

        protected virtual IEnumerable<DomainEvent> Given()
        {
            return new List<DomainEvent>();
        }

        protected virtual void Finally() { }

        protected abstract TCommand When();

        [SetUp]
        public void Setup()
        {
            Repository = new FakeRepository<TAggregateRoot>();
            CaughtException = new ThereWasNoExceptionButOneWasExpectedException();
            AggregateRoot = new TAggregateRoot();
            AggregateRoot.LoadFromHistory(Given());

            CommandHandler = BuildCommandHandler();
            SetupDependencies();
            try
            {
                CommandHandler.Handle(When());
                if (Repository.SavedAggregate == null)
                    PublishedEvents = AggregateRoot.GetChanges();
                else
                    PublishedEvents = Repository.SavedAggregate.GetChanges();
            }
            catch (Exception exception)
            {
                CaughtException = exception;
            }
            finally
            {
                Finally();
            }
        }

        private Handles<TCommand> BuildCommandHandler()
        {
            return Activator.CreateInstance(typeof(TCommandHandler), Repository) as TCommandHandler;
        }        
    }

    public class ThereWasNoExceptionButOneWasExpectedException : Exception { }
}