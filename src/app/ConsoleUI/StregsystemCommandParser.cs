using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using ostrich.ConsoleUI;

namespace ConsoleUI
{
    public class StregsystemCommandParser
    {
        private readonly IUserInterface ui;
        private readonly IStregsystem stregsystem;
        private readonly IDictionary<string, Action<Command>> commands;

        public StregsystemCommandParser(IUserInterface ui, IStregsystem stregsystem)
        {
            if (ui == null) throw new ArgumentNullException("ui");
            if (stregsystem == null) throw new ArgumentNullException("stregsystem");

            this.ui = ui;
            this.stregsystem = stregsystem;

            commands = new Dictionary<string, Action<Command>>
            {
                {":activate", ActivateProduct},
                {":deactivate", DeactivateProduct},
                {":list-products", ListProducts}, {":p", ListProducts},
                {":list-users", ListUsers}, {":u", ListUsers},
                {":quit", Quit}, {":q", Quit},
            };
        }

        public void Parse(string stringToParse)
        {
            if (string.IsNullOrWhiteSpace(stringToParse))
            {
                ui.DisplayGeneralError("Please command and I will try to obey.");
                return;
            }

            Command cmd = new Command(stringToParse);
            if (cmd.IsAdminCommand)
                ProcessAdminCommand(cmd);
            else if (cmd.RawArguments.Length == 1)
                ProcessSimpleUserCommand(cmd.RawArguments[0]);
            else
                ProcessQuickbuyCommand(cmd);
        }

        private void ProcessSimpleUserCommand(string userName)
        {
            try
            {
                User user = stregsystem.GetUser(userName);
                var transactions = stregsystem.GetTransactionList(user);
                var latestTransactions = transactions
                        .Where(t => t is BuyTransaction)
                        .Cast<BuyTransaction>()
                        .OrderBy(t => t.Date)
                        .Take(10);
                
                ui.DisplayUserInfo(user, latestTransactions);
            }
            catch (UserNotFoundException exception)
            {
                ui.DisplayUserNotFound(exception);
            }
        }

        private void ProcessQuickbuyCommand(Command cmd)
        {
            string userName = cmd.Name;

            int? argument = cmd.GetInt(1);

            if (!argument.HasValue)
            {
                string msg = string.Format("Please specify which product ID to buy. Example '{0} 11'", userName);
                ui.DisplayGeneralError(msg);
                return;
            }

            int productId = argument.Value;
            try
            {
                User user = stregsystem.GetUser(userName);
                Product product = stregsystem.GetProduct(productId);
                BuyTransaction transaction = stregsystem.BuyProduct(user, product);

                stregsystem.ExecuteTransaction(transaction);

                ui.DisplayUserBuysProduct(transaction);
            }
            catch (UserNotFoundException exception)
            {
                ui.DisplayUserNotFound(exception);
            }
            catch (RecordNotFoundException exception)
            {
                ui.DisplayProductNotFound(exception);
            }
            catch (InsufficientCreditsException exception)
            {
                ui.DisplayInsufficientCash(exception);
            }
            catch (ProductNotSaleableException exception)
            {
                ui.DisplayProductNotSaleable(exception);
            }
        }

        private void ProcessAdminCommand(Command cmd)
        {
            if (commands.ContainsKey(cmd.Name))
                commands[cmd.Name](cmd);
            else
                ui.DisplayAdminCommandNotFoundMessage(cmd);
        }

        private void ActivateProduct(Command command)
        {
            int productId = command.GetInt(1).Value;
            Product product = stregsystem.GetProduct(productId);
            product.Active = true;
        }

        private void DeactivateProduct(Command command)
        {
            int productId = command.GetInt(1).Value;
            Product product = stregsystem.GetProduct(productId);
            product.Active = false;
        }

        private void ListProducts(Command command)
        {
            var activeProducts = stregsystem.GetActiveProducts();
            Console.WriteLine("Active products:");
            foreach (var product in activeProducts)
                Console.WriteLine("{0,5}\t{1,50}\t{2,5}", product.ProductID, product.Name, product.Price);

        }

        private void ListUsers(Command command)
        {
            var users = stregsystem.GetUsers();
            Console.WriteLine("Users:");
            foreach (var product in users)
                Console.WriteLine("{0}", product);
        }

        private void Quit(Command command)
        {
            ui.Close();
        }
    }
}