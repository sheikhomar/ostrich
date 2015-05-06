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
        private readonly IDictionary<string, Action<int>> commands;

        public StregsystemCommandParser(IUserInterface ui, IStregsystem stregsystem)
        {
            if (ui == null) throw new ArgumentNullException("ui");
            if (stregsystem == null) throw new ArgumentNullException("stregsystem");

            this.ui = ui;
            this.stregsystem = stregsystem;

            commands = new Dictionary<string, Action<int>>
            {
                {":activate", ActivateProduct},
                {":deactivate", DeactivateProduct},
            };
        }

        public void Parse(string stringToParse)
        {
            if (string.IsNullOrWhiteSpace(stringToParse))
            {
                ui.DisplayGeneralError("Invalid command detected.");
                return;
            }

            Command cmd = new Command(stringToParse);
            if (cmd.IsAdminCommand)
            {
                ProcessAdminCommand(cmd);
            }
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
            int? argument = cmd.GetInt(1);

            if (":q".Equals(cmd.Name) || ":quit".Equals(cmd.Name))
                ui.Close();
            else if (commands.ContainsKey(cmd.Name) && argument.HasValue)
                commands[cmd.Name](argument.Value);
            else
                ui.DisplayAdminCommandNotFoundMessage(cmd);
        }

        private void ActivateProduct(int productId)
        {
            Product product = stregsystem.GetProduct(productId);
            if (product.Active)
                ui.DisplayGeneralError(String.Format("Product {0} is already active.", product.ProductID));
            else
                product.Active = true;
        }

        private void DeactivateProduct(int productId)
        {
            Product product = stregsystem.GetProduct(productId);
            if (!product.Active)
                ui.DisplayGeneralError(String.Format("Product '{0}' is not active.", product.ProductID));
            else
                product.Active = false;
        }
    }
}