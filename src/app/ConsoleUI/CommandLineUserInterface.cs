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
            do
            {
                var activeProducts = stregsystem.GetActiveProducts();
                Console.WriteLine("Active products:");
                foreach (var product in activeProducts)
                    Console.WriteLine("{0,5}\t{1,50}\t{2,5}", product.ProductID, product.Name, product.Price);

                Console.WriteLine();
                Console.Write("Your command is my will # ");
                parser.Parse(Console.ReadLine());
            } while (true);
        }

        public void DisplayUserNotFound()
        {
            throw new System.NotImplementedException();
        }

        public void DisplayProductNotFound()
        {
            throw new System.NotImplementedException();
        }

        public void DisplayUserInfo()
        {
            throw new System.NotImplementedException();
        }

        public void DisplayTooManyArgumentsError()
        {
            throw new System.NotImplementedException();
        }

        public void DisplayAdminCommandNotFoundMessage()
        {
            throw new System.NotImplementedException();
        }

        public void DisplayUserBuysProduct(BuyTransaction transaction)
        {
            throw new System.NotImplementedException();
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

        public void DisplayInsufficientCash()
        {
            throw new System.NotImplementedException();
        }

        public void DisplayGeneralError(string errorString)
        {
            Console.WriteLine(errorString);
        }
    }
}