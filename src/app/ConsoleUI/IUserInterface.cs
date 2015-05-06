using System.Collections.Generic;
using ostrich.ConsoleUI;

namespace ConsoleUI
{
    public interface IUserInterface
    {
        void DisplayUserNotFound(UserNotFoundException exception);
        void DisplayProductNotFound(RecordNotFoundException exception);
        void DisplayUserInfo(User user, IEnumerable<BuyTransaction> latestTransactions);
        void DisplayTooManyArgumentsError();
        void DisplayAdminCommandNotFoundMessage(Command cmd);
        void DisplayUserBuysProduct(BuyTransaction transaction);
        void DisplayUserBuysProduct(int count);
        void Close();
        void DisplayInsufficientCash(InsufficientCreditsException exception);
        void DisplayGeneralError(string errorString);
        void DisplayProductNotSaleable(ProductNotSaleableException exception);
        void DisplayCashInserted(InsertCashTransaction transaction);
    }
}