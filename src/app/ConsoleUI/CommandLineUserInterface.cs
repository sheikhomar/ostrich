using System;
using System.Collections.Generic;
using System.Data;
using ostrich.ConsoleUI;

namespace ConsoleUI
{
    public class CommandLineUserInterface : IUserInterface
    {
        private readonly IStregsystem stregsystem;

        public CommandLineUserInterface(IStregsystem stregsystem)
        {
            this.stregsystem = stregsystem;
        }

        public void Start(StregsystemCommandParser parser)
        {
            Console.WriteLine("Your wish is my command.");
            do
            {
                Console.Write("# ");
                parser.Parse(Console.ReadLine());
            } while (true);
        }

        public void DisplayUserNotFound(UserNotFoundException exception)
        {
            Console.WriteLine("User '{0}' was not found.", exception.UserName);
        }

        public void DisplayProductNotFound(int productId)
        {
            throw new NotImplementedException();
        }

        public void DisplayProductNotFound(RecordNotFoundException exception)
        {
            Console.WriteLine("Product '{0}' was not found.", exception.ProductID);
        }

        public void DisplayUserInfo(User user, IEnumerable<BuyTransaction> latestTransactions)
        {
            Console.WriteLine("User Details");
            Console.WriteLine(" Username: {0}\nFull name: {1}\nBalance: {2}", user.UserName, user.FullName, user.Balance);
            Console.WriteLine("Recent buy transactions");
            foreach (BuyTransaction transaction in latestTransactions)
            {
                Console.WriteLine(" -> {0}", transaction);
            }
        }

        public void DisplayTooManyArgumentsError()
        {
            throw new System.NotImplementedException();
        }

        public void DisplayAdminCommandNotFoundMessage(Command cmd)
        {
            Console.WriteLine("Administration command '{0}' is not valid.", cmd.Name);
        }

        public void DisplayUserBuysProduct(BuyTransaction transaction)
        {
            Console.WriteLine("User\n  {0}\nhas bought\n  {1}", transaction.User, transaction.Product);
        }

        public void DisplayUserBuysProduct(int count)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            // http://stackoverflow.com/questions/10286056/what-is-the-command-to-exit-a-console-application-in-c
            Environment.Exit(0);
       }

        public void DisplayInsufficientCash(InsufficientCreditsException exception)
        {
            Console.WriteLine("User {0} has insufficient funds to buy product {1} that costs {2}", 
                exception.User, exception.Product, exception.Product.Price);
        }

        public void DisplayGeneralError(string errorString)
        {
            Console.WriteLine(errorString);
        }

        public void DisplayProductNotSaleable(ProductNotSaleableException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}