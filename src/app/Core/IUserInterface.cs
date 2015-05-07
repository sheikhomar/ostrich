using System.Collections.Generic;
using ostrich.Core.Exceptions;

namespace ostrich.Core
{
    public interface IUserInterface
    {
        void DisplayUserNotFound(UserNotFoundException exception);
        void DisplayProductNotFound(ProductNotFoundException exception);
        void DisplayUserInfo(User user, IEnumerable<BuyTransaction> latestTransactions);
        void DisplayTooManyArgumentsError();
        void DisplayAdminCommandNotFoundMessage(Command cmd);
        void DisplayUserBuysProduct(BuyTransaction transaction);
        void Close();
        void DisplayInsufficientCash(InsufficientCreditsException exception);
        void DisplayGeneralError(string errorString);
        void DisplayProductNotSaleable(ProductNotSaleableException exception);
        void DisplayCashInserted(InsertCashTransaction transaction);
        void DisplaceProducts(IEnumerable<Product> products);
        void DisplayUsers(IEnumerable<User> users);
    }
}