using System;
using ostrich.Core.Exceptions;

namespace ostrich.Core
{
    public class QuickBuyController : Controller
    {
        public QuickBuyController(IUserInterface ui, IBackendSystem system) : base(ui, system)
        {
        }

        protected override void ProcessInternal(CommandArgumentCollection args)
        {
            if (args.Count == 2)
                BuySingle(args);
            else if (args.Count == 3)
                BuyMultiple(args);
            else
                throw new InvalidOperationException("Cannot process command with more than 3 arguments.");
        }

        protected void BuySingle(CommandArgumentCollection args)
        {
            string userName = args[0];
            int? argument = args.GetAsInt(1);

            if (!argument.HasValue)
            {
                string msg = string.Format("Please specify which product ID to buy. Example '{0} 11'", userName);
                UI.DisplayGeneralError(msg);
                return;
            }

            int productId = argument.Value;
            PerformBuy(userName, productId);
        }


        private void BuyMultiple(CommandArgumentCollection args)
        {
            string userName = args[0];
            int? quantity = args.GetAsInt(1);
            int? productId = args.GetAsInt(2);

            if (!quantity.HasValue && quantity.Value < 1)
            {
                UI.DisplayGeneralError("Please specify valid quantity.");
                return;
            }

            if (!productId.HasValue && productId.Value < 2)
            {
                UI.DisplayGeneralError("Please specify valid product ID.");
                return;
            }

            for (int i = 0; i < quantity.Value; i++)
            {
                bool isSuccess = PerformBuy(userName, productId.Value);
                if (!isSuccess)
                    break;
            }
        }

        private bool PerformBuy(string userName, int productId)
        {
            try
            {
                User user = System.GetUser(userName);
                Product product = System.GetProduct(productId);
                BuyTransaction transaction = System.BuyProduct(user, product);

                System.ExecuteTransaction(transaction);

                UI.DisplayUserBuysProduct(transaction);
            }
            catch (UserNotFoundException exception)
            {
                UI.DisplayUserNotFound(exception.UserName);
                return false;
            }
            catch (ProductNotFoundException exception)
            {
                UI.DisplayProductNotFound(exception.ProductID);
                return false;
            }
            catch (InsufficientCreditsException exception)
            {
                UI.DisplayInsufficientCash(exception.User, exception.Product);
                return false;
            }
            catch (ProductNotSaleableException exception)
            {
                UI.DisplayProductNotSaleable(exception.Product);
                return false;
            }

            return true;
        }
    }
}