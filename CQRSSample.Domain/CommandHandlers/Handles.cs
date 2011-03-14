// ReSharper disable InconsistentNaming

using CQRSSample.Commands;

namespace CQRSSample.Domain.CommandHandlers
{
    public interface Handles<in T> : Handles where T : Command
    { 
        void Handle(T command); 
    }

    public interface Handles{}
}

// ReSharper restore InconsistentNaming