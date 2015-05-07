using System;
using System.Collections.Generic;
using System.Linq;
using ostrich.Core;

namespace ostrich.ConsoleUI
{
    public class CommandLineInterface : IUserInterface
    {
        private bool running;

        public void Start(StregsystemCommandParser parser)
        {
            running = true;
            Console.WriteLine("Your wish is my command.");
            do
            {
                Console.Write("# ");
                parser.Parse(Console.ReadLine());
            } while (running);
        }

        public void DisplayUserNotFound(string userName)
        {
            Console.WriteLine("User '{0}' was not found.", userName);
        }

        public void DisplayProductNotFound(int productId)
        {
            Console.WriteLine("Product with ID {0} was not found.", productId);
        }

        public void DisplayUserInfo(User user, IEnumerable<BuyTransaction> latestTransactions)
        {
            Console.Write("User Details\n Username: {0}\n Full name: {1}\n Balance: {2}", 
                user.UserName, user.FullName, user.Balance);

            if (user.HasLowBalance)
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  (low balance)");
                Console.ForegroundColor = foregroundColor;
            }
            else
            {
                Console.WriteLine();
            }

            if (latestTransactions.Any())
            {
                Console.WriteLine("Recent buy transactions");
                foreach (BuyTransaction transaction in latestTransactions)
                {
                    Console.WriteLine(" -> {0}", transaction);
                }    
            }
        }

        public void DisplayTooManyArgumentsError()
        {
            Console.WriteLine("Your command contains too many arguments. Please try again.");
        }

        public void DisplayAdminCommandNotFoundMessage(string commandName)
        {
            Console.WriteLine("Administration command '{0}' is not valid.", commandName);
        }

        public void DisplayUserBuysProduct(BuyTransaction transaction)
        {
            Console.WriteLine("User\n  {0}\nhas bought\n  {1}", transaction.User, transaction.Product);
        }
        
        public void Close()
        {
            running = false;
        }

        public void DisplayInsufficientCash(User user, Product product)
        {
            Console.WriteLine("User '{0}' has insufficient funds to buy '{1}'.", 
                user.UserName, product.Name);
        }

        public void DisplayGeneralError(string errorString)
        {
            Console.WriteLine(errorString);
        }

        public void DisplayProductNotSaleable(Product product)
        {
            Console.WriteLine("Product '{0}' is not saleable.", product.ProductID);
        }

        public void DisplayCashInserted(InsertCashTransaction transaction)
        {
            Console.WriteLine("{0} has been inserted into the account of '{1}'.", 
                transaction.Amount, transaction.User.UserName);
        }

        public void DisplaceProducts(IEnumerable<Product> products)
        {
            Console.WriteLine("Active products:");
            Console.WriteLine("{0,-5}{1}\t{2,-50}\t{3,-10}", "ID", " ", "Products", "Price");
            foreach (var product in products)
            {
                Console.WriteLine("{0,5}{1}\t{2,-50}\t{3,10}",
                    product.ProductID,
                    product.CanBeBoughtOnCredit ? "*" : " ",
                    product.Name,
                    product.FormattedPrice);
            }
        }

        public void DisplayUsers(IEnumerable<User> users)
        {
            Console.WriteLine("Users:");
            foreach (var product in users)
                Console.WriteLine("{0}", product);
        }
    }
}