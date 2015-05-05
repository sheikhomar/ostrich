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
            var activeProducts = stregsystem.GetActiveProducts();
            foreach (var product in activeProducts)
            {
                Console.WriteLine("Product: {0}", product);
            }

            Console.WriteLine("Application started. Please type a command: ");
            string command = Console.ReadLine();
            parser.Parse(command);
            Console.ReadKey();
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