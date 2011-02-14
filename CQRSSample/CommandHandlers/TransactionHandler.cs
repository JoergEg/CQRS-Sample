using System;
using System.Transactions;
using CQRSSample.Commands;
using Raven.Client;

namespace CQRSSample.CommandHandlers
{
    public class TransactionHandler
    {
        //TODO: anstatt IDocumentStore könnte IUnitOfWork verwendet werden, damit CommandHandler keine Abhängigkeit auf RavenDB
        //haben. Implementierung von IUnitOfWork verwendet dann IDocumentStore.

        private readonly IDocumentStore _documentStore;

        public TransactionHandler(IDocumentStore unitOfWork)
        {
            _documentStore = unitOfWork;
        }

        public void Execute<TCommand, TCommandHandler>(TCommand command, TCommandHandler commandHandler) 
            where TCommandHandler : Handles<TCommand> 
            where TCommand : Command
        {
            //TODO: Transaktion funktioniert noch nicht

            //using (var tx = new TransactionScope())
            //{
                commandHandler.Handle(command);

            //    tx.Complete();
            //}



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