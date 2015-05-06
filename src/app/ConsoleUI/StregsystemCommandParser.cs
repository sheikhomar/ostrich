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
                {":activate", cmd => ToggleActivePropertyForProduct(cmd, true)},
                {":deactivate", cmd => ToggleActivePropertyForProduct(cmd, false)},
                {":list-products", ListProducts}, {":p", ListProducts},
                {":list-users", ListUsers}, {":u", ListUsers},
                {":addcredits", AddCredits},
                {":crediton", cmd => ToggleBuyOnCreditForProduct(cmd, true)},
                {":creditoff", cmd => ToggleBuyOnCreditForProduct(cmd, false)},
                {":quit", cmd => ui.Close()}, {":q", cmd => ui.Close()},
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
            else if (cmd.RawArguments.Length == 2)
                ProcessBuyCommand(cmd);
            else if (cmd.RawArguments.Length == 3)
                ProcessMultiBuyCommand(cmd);
            else
                ui.DisplayTooManyArgumentsError();
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

        private void ProcessBuyCommand(Command cmd)
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
            PerformBuy(userName, productId);
        }

        private bool PerformBuy(string userName, int productId)
        {
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
                return false;
            }
            catch (ProductNotFoundException exception)
            {
                ui.DisplayProductNotFound(exception);
                return false;
            }
            catch (InsufficientCreditsException exception)
            {
                ui.DisplayInsufficientCash(exception);
                return false;
            }
            catch (ProductNotSaleableException exception)
            {
                ui.DisplayProductNotSaleable(exception);
                return false;
            }

            return true;
        }

        private void ProcessMultiBuyCommand(Command cmd)
        {
            string userName = cmd.Name;
            int? quantity = cmd.GetInt(1);
            int? productId = cmd.GetInt(2);

            if (!quantity.HasValue && quantity.Value < 1)
            {
                ui.DisplayGeneralError("Please specify valid quantity.");
                return;
            }

            if (!productId.HasValue && productId.Value < 2)
            {
                ui.DisplayGeneralError("Please specify valid product ID.");
                return;
            }

            for (int i = 0; i < quantity.Value; i++)
            {
                bool isSuccess = PerformBuy(userName, productId.Value);
                if (!isSuccess)
                    break;
            }
        }

        private void ProcessAdminCommand(Command cmd)
        {
            if (commands.ContainsKey(cmd.Name))
                commands[cmd.Name](cmd);
            else
                ui.DisplayAdminCommandNotFoundMessage(cmd);
        }

        private void ToggleActivePropertyForProduct(Command command, bool isActive)
        {
            if (command.RawArguments.Length != 2)
            {
                ui.DisplayGeneralError("Wrong arguments. Must be like ':[de]activate <product-id>'");
                return;
            }

            int? productId = command.GetInt(1);
            if (productId == null || productId.Value < 1)
            {
                ui.DisplayGeneralError("Product ID must be a positive integer.");
                return;
            }

            try
            {
                Product product = stregsystem.GetProduct(productId.Value);
                product.Active = isActive;
            }
            catch (ProductNotFoundException exception)
            {
                ui.DisplayProductNotFound(exception);
            }
        }

        private void ListProducts(Command command)
        {
            var activeProducts = stregsystem.GetActiveProducts();
            ui.DisplaceProducts(activeProducts);
        }

        private void ListUsers(Command command)
        {
            var users = stregsystem.GetUsers();
            ui.DisplayUsers(users);
        }

        private void AddCredits(Command command)
        {
            if (command.RawArguments.Length != 3)
            {
                ui.DisplayGeneralError("Wrong arguments. Must be like ':addcredits <user-name> <product-id>'");
                return;
            }

            int? amount = command.GetInt(2);
            if (amount == null || amount.Value < 1) 
            { 
                ui.DisplayGeneralError("Amount must be a positive integer.");
                return;
            }

            try
            {
                string userName = command.RawArguments[1];
                User user = stregsystem.GetUser(userName);
                
                InsertCashTransaction transaction = stregsystem.AddCreditsToAccount(user, amount.Value);
                stregsystem.ExecuteTransaction(transaction);
                ui.DisplayCashInserted(transaction);
            }
            catch (UserNotFoundException exception)
            {
                ui.DisplayUserNotFound(exception);
            }
        }

        private void ToggleBuyOnCreditForProduct(Command command, bool canBeBoughtOnCredit)
        {
            if (command.RawArguments.Length != 2)
            {
                ui.DisplayGeneralError("Wrong arguments. Must be like ':credit[on|off] <product-id>'");
                return;
            }

            int? productId = command.GetInt(1);
            if (productId == null || productId.Value < 1)
            {
                ui.DisplayGeneralError("Product ID must be a positive integer.");
                return;
            }

            try
            {
                Product product = stregsystem.GetProduct(productId.Value);
                product.CanBeBoughtOnCredit = canBeBoughtOnCredit;
            }
            catch (ProductNotFoundException exception)
            {
                ui.DisplayProductNotFound(exception);
            }
        }
    }
}