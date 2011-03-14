using System;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CQRSSample.Commands;
using FluentValidation;
using FluentValidation.Results;

namespace CQRSSample.WpfClient.ApplicationFramework
{
    public class ValidatingCommand<T> : DynamicCommand<T>, IDataErrorInfo where T : Command
    {
        private readonly IValidator<T> _validator;

        public ValidatingCommand(T command, IValidator<T> validator)
            : base(command)
        {
            _validator = validator;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var ex = Expression.Property(Expression.Constant(InnerCommand), binder.Name);
            result = Expression.Lambda(ex).Compile().DynamicInvoke();
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var method = InnerCommand.GetType().GetProperty(binder.Name).GetSetMethod();
            method.Invoke(InnerCommand, new[] { value });

            return true;
        }

        /// <summary>
        /// Validates the command
        /// </summary>
        public virtual ValidationResult Validate()
        {
            return _validator.Validate(InnerCommand);
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
}