using System;

namespace CQRSSample.Commands
{
    public abstract class Command
    {
        public readonly Guid Id;

        protected Command(Guid id)
        {
            Id = id;
        }
    }
}