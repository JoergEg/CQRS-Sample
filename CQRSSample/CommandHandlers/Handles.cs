// ReSharper disable InconsistentNaming

using CQRSSample.Commands;

namespace CQRSSample.CommandHandlers
{
    public interface Handles<T> where T : Command
    { 
        void Handle(T command); 
    }
}

// ReSharper restore InconsistentNaming