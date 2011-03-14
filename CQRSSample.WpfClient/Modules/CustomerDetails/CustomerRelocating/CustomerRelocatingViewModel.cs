using System;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Caliburn.Micro;
using CQRSSample.Commands;
using CQRSSample.Infrastructure;
using CQRSSample.ReadModel;
using FluentValidation;
using FluentValidation.Results;

namespace CQRSSample.WpfClient.Modules.CustomerDetails.CustomerRelocating
{
    public class CustomerRelocatingViewModel : ValidatingScreenWithCommand<RelocatingCustomerCommand>
    {
        private readonly IBus _bus;
        private readonly IEventAggregator _eventAggregator;
        private readonly IReadRepository _readRepository;

        public CustomerRelocatingViewModel(IBus bus, IEventAggregator eventAggregator, IReadRepository readRepository)
        {
            _bus = bus;
            _eventAggregator = eventAggregator;
            _readRepository = readRepository;

            Validator = new RelocatingCustomerValidator();
        }

        public void WithCustomer(Guid customerId)
        {
            ViewModel = _readRepository.GetById<CustomerListDto>(customerId);
            Command = new ValidatingCommand<RelocatingCustomerCommand>(new RelocatingCustomerCommand(ViewModel.Id), Validator);
        }

        public CustomerListDto ViewModel { get; private set; }

        public void Save()
        {
            //TODO: Validation
            if(!Validate().IsValid)
                return;

            //important: send command over bus
            _bus.Send(Command.InnerCommand);

            //signal for UI - change view
            _eventAggregator.Publish(new CustomerRelocatingSavedEvent());
        }
    }

    public class RelocatingCustomerValidator : AbstractValidator<RelocatingCustomerCommand>
    {
        public RelocatingCustomerValidator()
        {
            RuleFor(command => command.City).NotEmpty().NotNull();
        }
    }

    public class ValidatingCommand<T> : DynamicObject, IDataErrorInfo where T : Command
    {
        private readonly dynamic _innerCommand;

        public ValidatingCommand(T command, IValidator<T> validator)
        {
            _innerCommand = command;
            Validator = validator;
        }

        public T InnerCommand{ get { return _innerCommand; }}

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var ex = Expression.Property(Expression.Constant(InnerCommand), binder.Name);
            result = Expression.Lambda(ex).Compile().DynamicInvoke();
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var method = InnerCommand.GetType().GetProperty(binder.Name).GetSetMethod(); 
            //var setter = (Action<Command, object>)Delegate.CreateDelegate(typeof(Action<Command, object>), method);
            method.Invoke(InnerCommand, new[]{value} );

            //setter(InnerCommand, value);
            return true;
        }

        public IValidator<T> Validator { get; private set; }

        /// <summary>
        /// Validates the command
        /// </summary>
        public virtual ValidationResult Validate()
        {
            return Validator.Validate(InnerCommand);
        }

        public string this[string columnName]
        {
            get
            {
                var validationResults = Validate();

                if (validationResults == null) return string.Empty;

                var columnResults = validationResults.Errors.FirstOrDefault(x => string.Compare(x.PropertyName, columnName, true) == 0);

                return columnResults != null ? columnResults.ErrorMessage : string.Empty;
            }
        }

        public string Error
        {
            get
            {
                var message = new StringBuilder();

                foreach (var validationFailure in Validate().Errors)
                {
                    message.Append(validationFailure.ErrorMessage);
                    message.Append(Environment.NewLine);
                }

                return message.ToString();
            }
        }
    }

    public class ValidatingScreenWithCommand<T> : Screen, IDataErrorInfo where T : Command
    {
        public ValidatingScreenWithCommand()
        {
            var c = new RelocatingCustomerCommand(Guid.NewGuid());
        }

        public ValidatingCommand<T> Command { get; protected set; }

        public IValidator<T> Validator { get; set; }

        /// <summary>
        /// Validates the command
        /// </summary>
        protected virtual ValidationResult Validate()
        {
            return Command.Validate();
        }

        public string this[string columnName]
        {
            get
            {
                var validationResults = Validate();

                if (validationResults == null) return string.Empty;

                var columnResults = validationResults.Errors.FirstOrDefault(x => string.Compare(x.PropertyName, columnName, true) == 0);

                return columnResults != null ? columnResults.ErrorMessage : string.Empty;
            }
        }

        public string Error
        {
            get
            {
                var message = new StringBuilder();

                foreach (var validationFailure in Validate().Errors)
                {
                    message.Append(validationFailure.ErrorMessage);
                    message.Append(Environment.NewLine);
                }

                return message.ToString();
            }
        }
    }

    public class CustomerRelocatingSavedEvent
    {
    }
}