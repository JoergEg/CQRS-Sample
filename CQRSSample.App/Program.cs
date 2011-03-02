//based on samples by Mark Nijhof: https://github.com/MarkNijhof/Fohjin
//Greg Young: http://github.com/gregoryyoung/m-r
//and http://dddsamplenet.codeplex.com/
//using EventStore by Jonathan Oliver https://github.com/joliver/EventStore

using System;
using Castle.Windsor;
using CQRSSample.Commands;
using CQRSSample.Infrastructure;
using CQRSSample.ReadModel;
using Raven.Client;
using Raven.Client.Document;

namespace CQRSSample.App
{
    class Program
    {
        static void Main()
        {
            var store = new DocumentStore{ Url = "http://localhost:8080" };
            store.Initialize();

            //run RavenDB InMemory
            //var store = new EmbeddableDocumentStore {RunInMemory = true};

            IWindsorContainer container = null;
            try
            {
                container = BootStrapper.BootStrap();

                var bus = container.Resolve<IBus>();
                var aggregateId = Guid.NewGuid();

                //create customer (Write/Command)
                CreateCustomer(bus, aggregateId);

                //Customer relocating (Write/Command)
                RelocateCustomer(bus, aggregateId);

                //show all customers [in memory] (Read/Query)
                ShowCustomerListView(store);
            }
            catch(System.Net.WebException ex)
            {
                Console.WriteLine(@"Unable to connect to RavenDB Server. Have you started 'RavenDB\Server\Raven.Server.exe'?");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler: " + ex.Message);
            }
            finally
            {
                if (container != null) container.Dispose();
            }
            Console.WriteLine("Press any key to finish.");
            Console.ReadLine();
        }

        private static void ShowCustomerListView(IDocumentStore store)
        {
            using (var session = store.OpenSession())
            {
                foreach(var dto in session.Query<CustomerListDto>())
                {
                    Console.WriteLine(dto.Name + " now living in " + dto.City + " (" + dto.Id + ")");
                    Console.WriteLine("---");
                }
            }
        }

        private static void RelocateCustomer(IBus bus, Guid aggregateId)
        {
            bus.Send(new RelocatingCustomerCommand(aggregateId, "Messestraße",  "2", "4444", "Linz"));

            Console.WriteLine("Customer relocated. Press any key to show list of customers.");
            Console.ReadLine();
        }

        private static void CreateCustomer(IBus bus, Guid aggregateId)
        {
            bus.Send(new CreateCustomerCommand(aggregateId, "Jörg Egretzberger", "Meine Straße", "1", "1010", "Wien", "01/123456"));
            Console.WriteLine("Customer created. Press any key to relocate customer.");
            Console.ReadLine();
        }
    }
}
