using System;
using ostrich.Core.Exceptions;

namespace ostrich.Core
{
    public class QuickBuyController : Controller
    {
        public const string InvalidProductIdMessage = "Please specify valid product ID.";
        public const string InvalidQuantityMessage = "Please specify valid quantity.";

        public QuickBuyController(IUserInterface ui, IBackendSystem system) : base(ui, system)
        {
        }

        protected override void ProcessInternal(CommandArgumentCollection args)
        {
            string userName = args[0];
            int? quantity = null;
            int? productId = null;

            if (args.Count == 2)
            {
                quantity = 1;
                productId = args.GetAsInt(1);
            }
            else if (args.Count == 3)
            {
                quantity = args.GetAsInt(1);
                productId = args.GetAsInt(2);
            }
            else
                throw new InvalidOperationException("Invalid arguments provided.");

            PerformBuy(userName, productId, quantity);
        }

        private void PerformBuy(string userName, int? productId, int? quantity)
        {
            if (quantity == null || quantity.Value < 1)
            {
                UI.DisplayGeneralError(InvalidQuantityMessage);
                return;
            }

            if (productId == null  || productId.Value < 1)
            {
                UI.DisplayGeneralError(InvalidProductIdMessage);
                return;
            }

            try
            {
                User user = System.GetUser(userName);
                Product product = System.GetProduct(productId.Value);
                BuyTransaction transaction = null;
                for (int i = 0; i < quantity; i++)
                {
                    transaction = System.BuyProduct(user, product);
                    System.ExecuteTransaction(transaction);    
                }
                
                if (quantity == 1)
                    UI.DisplayUserBuysProduct(transaction);
                else
                    UI.DisplayUserBuysProduct(product, user, quantity.Value);
            }
            catch (UserNotFoundException exception)
            {
                UI.DisplayUserNotFound(exception.UserName);
            }
            catch (ProductNotFoundException exception)
            {
                UI.DisplayProductNotFound(exception.ProductID);
            }
            catch (InsufficientCreditsException exception)
            {
                UI.DisplayInsufficientCash(exception.User, exception.Product);
            }
            catch (ProductNotSaleableException exception)
            {
                UI.DisplayProductNotSaleable(exception.Product);
            }
            catch (BalanceUnderflowException exception)
            {
                UI.DisplayGeneralError(exception.Message);
            }
        }
    }
}