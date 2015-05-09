using System.IO;
using System.Text;
using ostrich.Core;

namespace ostrich.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: assume that file encoding is Encoding.Default
            var reader = new StreamReader("products.csv", Encoding.Default);
            var importer = new ProductCatalogImporter(reader);
            
            var productCatalog = importer.Import();

            var transactionFile = new TransactionFile("transactions.log");
            var transactions = new TransactionManager(transactionFile);

            var userRepository = new UserRepository();
            userRepository.Add(new User(1, "Joakim", "Von And", "b") { Balance = int.MaxValue });
            userRepository.Add(new User(2, "Anders", "And", "a") { Balance = 6000 });

            IBackendSystem backendSystem = new DefaultBackendSystem(productCatalog, userRepository, transactions);
            CommandLineInterface cli = new CommandLineInterface();
            CommandParser parser = new CommandParser(cli, backendSystem);
            cli.Start(parser);
        }
    }
}
