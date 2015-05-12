using System;
using System.Collections.Generic;
using ostrich.Core.Exceptions;

namespace ostrich.Core.Processors
{
    public class AdministrationCommandProcessor : CommandProcessor
    {
        private readonly IDictionary<string, Action<CommandArgumentCollection>> commands;

        public AdministrationCommandProcessor(IUserInterface ui, IBackendSystem system)
            : base(ui, system)
        {
            commands = new Dictionary<string, Action<CommandArgumentCollection>>
            {
                {":activate",       args => ToggleActivePropertyForProduct(args, true)},
                {":deactivate",     args => ToggleActivePropertyForProduct(args, false)},
                {":crediton",       args => ToggleBuyOnCreditForProduct(args, true)},
                {":creditoff",      args => ToggleBuyOnCreditForProduct(args, false)},
                {":list-products",  _ => ListProducts()}, 
                {":p",              _ => ListProducts()},
                {":list-users",     _ => ListUsers()}, 
                {":u",              _ => ListUsers()},
                {":quit",           _ => UI.Close()}, 
                {":q",              _ => UI.Close()},
                {":addcredits",     AddCredits}
            };
        }

        protected override void ProcessInternal(CommandArgumentCollection args)
        {
            string command = args[0];
            if (commands.ContainsKey(command))
                commands[command](args);
            else
                UI.DisplayAdminCommandNotFoundMessage(command);
        }

        private void ToggleActivePropertyForProduct(CommandArgumentCollection args, bool isActive)
        {
            if (args.Count != 2)
            {
                UI.DisplayGeneralError("Wrong arguments. Must be like ':[de]activate <product-id>'");
                return;
            }

            var productId = args.GetAsInt(1);
            if (productId == null || productId.Value < 1)
            {
                UI.DisplayGeneralError("Product ID must be a positive integer.");
                return;
            }

            try
            {
                Product product = System.GetProduct(productId.Value);
                if (product is SeasonalProduct)
                    UI.DisplayGeneralError("Cannot activate or deactivate seasonal product.");
                else
                    product.Active = isActive;
            }
            catch (ProductNotFoundException exception)
            {
                UI.DisplayProductNotFound(exception.ProductID);
            }
        }

        private void ToggleBuyOnCreditForProduct(CommandArgumentCollection args, bool canBeBoughtOnCredit)
        {
            if (args.Count != 2)
            {
                UI.DisplayGeneralError("Wrong arguments. Must be like ':credit[on|off] <product-id>'");
                return;
            }

            int? productId = args.GetAsInt(1);
            if (productId == null || productId.Value < 1)
            {
                UI.DisplayGeneralError("Product ID must be a positive integer.");
                return;
            }

            try
            {
                Product product = System.GetProduct(productId.Value);
                product.CanBeBoughtOnCredit = canBeBoughtOnCredit;
            }
            catch (ProductNotFoundException exception)
            {
                UI.DisplayProductNotFound(exception.ProductID);
            }
        }

        private void AddCredits(CommandArgumentCollection args)
        {
            if (args.Count != 3)
            {
                UI.DisplayGeneralError("Wrong arguments. Must be like ':addcredits <user-name> <amount>'");
                return;
            }

            string userName = args[1];
            int? amount = args.GetAsInt(2);

            if (amount == null || amount.Value < 1)
            {
                UI.DisplayGeneralError("Amount must be a positive integer.");
                return;
            }

            try
            {
                User user = System.GetUser(userName);

                InsertCashTransaction transaction = System.AddCreditsToAccount(user, amount.Value);
                System.ExecuteTransaction(transaction);
                UI.DisplayCashInserted(transaction);
            }
            catch (UserNotFoundException exception)
            {
                UI.DisplayUserNotFound(exception.UserName);
            }
            catch (BalanceOverflowException exception)
            {
                UI.DisplayBalanceOverflow(exception.User);
            }
        }

        private void ListProducts()
        {
            UI.DisplaceProducts(System.GetActiveProducts());
        }

        private void ListUsers()
        {
            UI.DisplayUsers(System.GetUsers());
        }
    }
}