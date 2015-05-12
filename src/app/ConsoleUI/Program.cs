using System;
using System.IO;
using System.Text;
using ostrich.Core;

namespace ostrich.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var productCatalog = GetProductCatalog();
            var transactions = GetTransactionManager();
            var userRepository = GetUserRepository();

            IBackendSystem backendSystem = new DefaultBackendSystem(productCatalog, userRepository, transactions);
            CommandLineInterface cli = new CommandLineInterface();
            CommandParser parser = new CommandParser(cli, backendSystem);
            cli.Start(parser);
        }

        private static UserRepository GetUserRepository()
        {
            var userRepository = new UserRepository();
            userRepository.Add(new User(1, "Joakim", "Von And", "b") {Balance = int.MaxValue});
            userRepository.Add(new User(2, "Anders", "And", "a") {Balance = 6000});
            return userRepository;
        }

        private static ProductCatalog GetProductCatalog()
        {
            var reader = new StreamReader("products.csv", Encoding.GetEncoding(1252));
            var importer = new ProductCatalogImporter(reader);
            var productCatalog = importer.Import();
            productCatalog.Add(new SeasonalProduct(2000, "Forårsfest")
            {
                SeasonStartsAt = new DateTime(2015, 4, 1),
                SeasonEndsAt = new DateTime(2015, 7, 15),
                CanBeBoughtOnCredit = true,
                Price = 14900
            });
            productCatalog.Add(new SeasonalProduct(2001, "Sommerfest")
            {
                SeasonStartsAt = new DateTime(2015, 7, 15),
                SeasonEndsAt = new DateTime(2015, 8, 1),
                Price = 29900

            });
            return productCatalog;
        }

        private static TransactionManager GetTransactionManager()
        {
            ITransactionStore transactionFile = new TransactionFileStore("transactions.log");
            return new TransactionManager(transactionFile);
        }
    }
}
