using System;
using System.Collections.Generic;
using System.IO.Pipes;
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
                ProcessAdminCommand(cmd);
            else
                ProcessQuickbuyCommand(cmd);
        }

        private void ProcessQuickbuyCommand(Command cmd)
        {
            string userName = cmd.Name;
            
            if (!cmd.Argument.HasValue)
            {
                string msg = string.Format("Please specify which product ID to buy. Example '{0} 11'", userName);
                ui.DisplayGeneralError(msg);
                return;
            }

            int productId = cmd.Argument.Value;
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
            if (":q".Equals(cmd.Name) || ":quit".Equals(cmd.Name))
                ui.Close();
            else if (commands.ContainsKey(cmd.Name) && cmd.Argument.HasValue)
                commands[cmd.Name](cmd.Argument.Value);
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