using CQRSSample.Commands;

namespace CQRSSample.Domain.CommandHandlers
{
    public class TransactionHandler
    {
        //TODO: use IUnitOfWork instead of IDocumentStore, so the CommandHandler has no dependency on RavenDB
        //Implementation of IUnitOfWork uses IDocumentStore


        public void Execute<TCommand, TCommandHandler>(TCommand command, TCommandHandler commandHandler) 
            where TCommandHandler : Handles<TCommand> 
            where TCommand : Command
        {
            //using (var tx = new TransactionScope())
            //{
            //    commandHandler.Handle(command);

            //    tx.Complete();
            //}

            //TODO
            commandHandler.Handle(command);

            //try
            //{
            //    commandHandler.Execute(command);
            //    _unitOfWork.Commit();
            //}
            //catch (Exception Ex)
            //{
            //    _unitOfWork.Rollback();
            //    throw;
            //}
        }
    }
}