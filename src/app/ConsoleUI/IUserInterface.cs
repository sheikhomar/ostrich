using ostrich.ConsoleUI;

namespace ConsoleUI
{
    public interface IUserInterface
    {
        void DisplayUserNotFound();
        void DisplayProductNotFound();
        void DisplayUserInfo();
        void DisplayTooManyArgumentsError();
        void DisplayAdminCommandNotFoundMessage();
        void DisplayUserBuysProduct(BuyTransaction transaction);
        void DisplayUserBuysProduct(int count);
        void Close();
        void DisplayInsufficientCash();
        void DisplayGeneralError(string errorString);
    }
}