using System.Collections.Generic;
using ostrich.Core.Exceptions;

namespace ostrich.Core
{
    public interface IUserInterface
    {
        void DisplayUserNotFound(string userName);
        void DisplayProductNotFound(int productId);
        void DisplayUserInfo(User user, IEnumerable<BuyTransaction> latestTransactions);
        void DisplayTooManyArgumentsError();
        void DisplayAdminCommandNotFoundMessage(string cmd);
        void DisplayUserBuysProduct(BuyTransaction transaction);
        void DisplayUserBuysProduct(Product product, User user, int quantity);
        void Close();
        void DisplayInsufficientCash(User user, Product product);
        void DisplayGeneralError(string errorString);
        void DisplayProductNotSaleable(Product product);
        void DisplayCashInserted(InsertCashTransaction transaction);
        void DisplaceProducts(IEnumerable<Product> products);
        void DisplayUsers(IEnumerable<User> users);
        void DisplayBalanceOverflow(User user);
    }
}